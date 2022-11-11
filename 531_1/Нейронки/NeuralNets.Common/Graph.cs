using System.Text;
using System.Text.Json.Serialization;

namespace NeuralNets.Common
{
    public class Graph
    {
        public Graph()
        {
            Vertexes = new HashSet<Vertex>();
            Arcs = new List<Arc>();
        }

        public HashSet<Vertex> Vertexes { get; set; }
        public List<Arc> Arcs { get; set; }

        public Dictionary<Vertex, List<Vertex>> ToAdjVector()
        {
            var result = new Dictionary<Vertex, List<Vertex>>();

            foreach (var vertex in Vertexes)
            {
                result.Add(vertex, new List<Vertex>());
            }

            foreach (var arc in Arcs)
            {
                result[arc.From].Add(arc.To);
            }

            return result;
        }

        public Dictionary<Vertex, List<Vertex>> ToAdjVectorInversed()
        {
            var result = new Dictionary<Vertex, List<Vertex>>();

            foreach (var vertex in Vertexes)
            {
                result.Add(vertex, new List<Vertex>());
            }

            foreach (var arc in Arcs)
            {
                result[arc.To].Add(arc.From);
            }

            return result;
        }

        public void CalculateValues(IEnumerable<Vertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                CalculateValue(vertex);
            }
        }

        public void CalculateValue(Vertex vertex)
        {
            if (vertex.Operation == Operation.Const)
            {
                return;
            }

            var inversedVector = ToAdjVectorInversed();

            calculateValueInternal(vertex);

            void calculateValueInternal(Vertex vertex)
            {
                if (vertex.Value is not null)
                {
                    return;
                }

                if (vertex.Operation == Operation.Const)
                {
                    return;
                }

                foreach (var from in inversedVector[vertex])
                {
                    calculateValueInternal(from);
                }

                if (vertex.Operation == Operation.Exp)
                {
                    if (inversedVector[vertex].Count != 1)
                    {
                        throw new ArgumentException("Вершина с операцией exp должна иметь ровно одну входящую дугу");
                    }

                    vertex.Value = Math.Exp(inversedVector[vertex].First().Value!.Value);
                }

                if (vertex.Operation == Operation.Summ)
                {
                    vertex.Value = 0;
                    foreach (var from in inversedVector[vertex])
                    {
                        vertex.Value += from.Value!.Value;
                    }
                }

                if (vertex.Operation == Operation.Multyply)
                {
                    vertex.Value = 1;
                    foreach (var from in inversedVector[vertex])
                    {
                        vertex.Value *= from.Value!.Value;
                    }
                }
            }
        }

        public (bool success, string error) AddOperations(string[] operationDescriptions)
        {
            foreach (var operationDescription in operationDescriptions)
            {
                var splited = operationDescription.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim(' ')).ToArray();
                if (splited.Length != 2)
                {
                    return (false, $"Неверная строка {operationDescription}");
                }

                var vertex = Vertexes.FirstOrDefault(x => x.Name == splited[0]);
                if (vertex is null)
                {
                    return (false, $"Неизвестная вершина {splited[0]}");
                }

                var operation = Operation.Unknown;
                switch (splited[1].ToLower())
                {
                    case "+":
                        operation = Operation.Summ;
                        break;
                    case "*":
                        operation = Operation.Multyply;
                        break;
                    case "exp":
                        operation = Operation.Exp;
                        break;
                    default:
                        break;
                }

                if (double.TryParse(splited[1], out var constValue))
                {
                    operation = Operation.Const;
                }

                if (operation == Operation.Unknown)
                {
                    return (false, $"Неизвестная операция {splited[1]}");
                }

                vertex.Operation = operation;
                vertex.Value = operation == Operation.Const
                    ? constValue
                    : null;
            }

            if (Vertexes.Any(x => x.Operation is null))
            {
                return (false, $"В файле не содержится операция для вершины {Vertexes.First(x => x.Operation is null).Name}");
            }

            var inversedVector = ToAdjVectorInversed();
            foreach (var vertex in Vertexes)
            {
                if (!inversedVector[vertex].Any() && vertex.Operation != Operation.Const)
                {
                    return (false, $"Вершина {vertex.Name} является стоком, операция для неё может быть только const");
                }
            }

            foreach (var vertex in Vertexes)
            {
                if (inversedVector[vertex].Any() && vertex.Operation == Operation.Const)
                {
                    return (false, $"Вершина {vertex.Name} не является стоком и не может иметь операцию const");
                }
            }

            foreach (var vertex in Vertexes)
            {
                if (inversedVector[vertex].Count > 1 && vertex.Operation == Operation.Exp)
                {
                    return (false, $"Вершина {vertex.Name} имеет степень захода > 1 и не может иметь операцию exp");
                }
            }

            return (true, string.Empty);
        }

        public List<(Vertex stock, string result)> ToBracketSequence()
        {
            var result = new List<(Vertex vertex, string result)>();

            var vector = ToAdjVector();
            var inverseVector = ToAdjVectorInversed();
            var stocks = vector
                .Where(x => !x.Value.Any())
                .Select(x => x.Key)
                .ToList();

            foreach (var stock in stocks)
            {
                var sb = new StringBuilder();
                AddVertex(stock, sb);
                result.Add((stock, sb.ToString()));
            }

            return result;

            void AddVertex(Vertex vertex, StringBuilder stringBuilder)
            {
                stringBuilder.Append(vertex.Name);

                if (inverseVector[vertex].Any())
                {
                    stringBuilder.Append('(');

                    foreach (var from in inverseVector[vertex])
                    {
                        AddVertex(from, stringBuilder);
                        stringBuilder.Append(',');
                    }

                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    stringBuilder.Append(')');
                }
            }
        }

        public bool HasCycle()
        {
            var g = ToAdjVector();

            var color = new Dictionary<Vertex, int>();
            var prevs = new Dictionary<Vertex, Vertex?>();

            foreach (var kvp in g)
            {
                color.Add(kvp.Key, 0);
                prevs.Add(kvp.Key, null);
            }

            foreach (var kvp in g)
            {
                if (color[kvp.Key] == 0)
                {
                    if (dfs(kvp.Key))
                    {
                        return true;
                    }
                }
            }

            return false;

            bool dfs(Vertex vertex)
            {
                color[vertex] = 1;

                foreach (var to in g[vertex])
                {
                    if (color[to] == 0)
                    {
                        prevs[to] = vertex;

                        if (dfs(to))
                        {
                            return true;
                        }
                    }
                    else if (color[to] == 1)
                    {
                        return true;
                    }
                }
                color[vertex] = 2;
                return false;
            }
        }

        public static (bool success, string error) TryCreateByStringDescription(string descriprion, out Graph result)
        {
            result = new Graph();

            var splitedArr = descriprion.Split('(', ')').Select(x => x.Trim(' ', ',')).Where(x => !string.IsNullOrEmpty(x)).ToArray();

            foreach (var splited in splitedArr)
            {
                var arcSplited = splited.Split(',').Select(x => x.Trim(' ')).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (arcSplited.Length != 3)
                {
                    return (false, $"Не удалось распарсить кортеж {splited}");
                }

                if (!int.TryParse(arcSplited[2], out var order) || order < 0)
                {
                    return (false, $"Ожидался целочисленный порядковый номер в кортеже {splited}. Актуальное значение: {arcSplited[2]}");
                }

                var from = new Vertex(arcSplited[0]);
                var to = new Vertex(arcSplited[1]);

                if (!result.Vertexes.TryGetValue(from, out var fromOld))
                {
                    result.Vertexes.Add(from);
                }
                else
                {
                    from = fromOld;
                }

                if (!result.Vertexes.TryGetValue(to, out var toOld))
                {
                    result.Vertexes.Add(to);
                }
                else
                {
                    to = toOld;
                }

                var arc = new Arc(from, to, order);
                if (result.Arcs.Contains(arc))
                {
                    return (false, $"Повторяющаяся дуга {splited}");
                }

                result.Arcs.Add(arc);
            }

            result.Arcs = result.Arcs.OrderBy(x => (x.To, x.Order, x.From)).ToList();

            var groups = result.Arcs.GroupBy(x => x.To);
            foreach (var group in groups)
            {
                var sorted = group.OrderBy(x => x.Order);

                var etalonOrder = 1;
                foreach (var item in sorted)
                {
                    if (item.Order != etalonOrder)
                    {
                        return (false, $"Номера дуг в списке входящих в вершину {item.To.Name} не соответствуют правильной нумерации (1, 2, 3, ...)");
                    }

                    etalonOrder++;
                }
            }

            if (result.HasCycle())
            {
                return (false, "Входной граф содержит цикл");
            }

            return (true, string.Empty);
        }
    }

    public class Vertex : IComparable<Vertex>
    {
        public string Name { get; set; }
        [JsonIgnore]
        public Operation? Operation { get; set; }
        [JsonIgnore]
        public double? Value { get; set; }

        public Vertex()
        {

        }

        public Vertex(string name)
        {
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vertex vertex &&
                   Name == vertex.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public int CompareTo(Vertex? other)
        {
            return Name.CompareTo(other.Name);
        }
    }

    public class Arc
    {
        public Arc()
        {

        }

        public Arc(Vertex from, Vertex to, int order)
        {
            From = from;
            To = to;
            Order = order;
        }

        public Vertex From { get; set; }
        public Vertex To { get; set; }
        public int Order { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Arc arc &&
                   EqualityComparer<Vertex>.Default.Equals(From, arc.From) &&
                   EqualityComparer<Vertex>.Default.Equals(To, arc.To) &&
                   Order == arc.Order;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To, Order);
        }
    }

    public enum Operation
    {
        Unknown = 0,
        Summ = 1,
        Multyply = 2,
        Exp = 3,
        Const = 4
    }
}