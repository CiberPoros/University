namespace NeuralNets.Programs.GraphSerializer
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
}
