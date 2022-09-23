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

        public static bool TryCreateByStringDescription(string descriprion, out Graph result)
        {
            result = new Graph();

            var splitedArr = descriprion.Split('(', ')').Select(x => x.Trim(' ', ',')).Where(x => !string.IsNullOrEmpty(x)).ToArray();

            foreach (var splited in splitedArr)
            {
                var arcSplited = splited.Split(',').Select(x => x.Trim(' ')).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (arcSplited.Length != 3)
                {
                    return false;
                }

                if (!int.TryParse(arcSplited[2], out var order) || order < 0)
                {
                    return false;
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
                    return false;
                }

                result.Arcs.Add(arc);
            }

            result.Arcs = result.Arcs.OrderBy(x => (x.From, x.To, x.Order)).ToList();

            return true;
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
