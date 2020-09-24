using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace GraphVisual
{
    public class Graph : IToolTip
    {
        public string Title { get; private set; }
        public int VertexId { get; private set; }
        public int LinkId { get; private set; }
        public int LinkCount { get; private set; }
        public List<Vertex> Vertices { get; private set; } = new List<Vertex>();
        public Point GraphPoint { get; set; }
        public double FigureWidth { get; private set; }
        public double FigureHeight { get; private set; }
        public GraphToolTip<Graph> toolTip { get; set; }

        public Graph(double figureWidth, double figureHeight)
        {
            FigureWidth = figureWidth;
            FigureHeight = figureHeight;
            toolTip = new GraphToolTip<Graph>(this);
        }
        public Graph(string title, Point graphPoint, double figureWidth, double figureHeight)
        {
            Title = title;
            GraphPoint = graphPoint;
            FigureWidth = figureWidth;
            FigureHeight = figureHeight;
            toolTip = new GraphToolTip<Graph>(this);
        } // Graph

        public int GetVerticesCount() => Vertices.Count;

        void Clear()
        {
            VertexId = 0;
            LinkId = 0;
            LinkCount = 0;
            Vertices.Clear();
            toolTip.ChangeToolTip(this);
        } // Clear

        public void AddVertex(string title, int weight, Point vertexPoint)
        {
            if (Exist(title))
                throw new Exception($"Vertex with name \"{title}\" already exist.");

            Vertex vertex = new Vertex(VertexId++, title, weight, vertexPoint, FigureWidth, FigureHeight);
            Vertices.Add(vertex);
            toolTip.ChangeToolTip(this);
        } // AddVertex

        public void AddLink(string title, int weight, string from, string to)
        {
            if (from == to) throw new Exception($"Vertex cant be linked with himself.");
            Vertex vertexFrom = FindVertex(from);
            if (vertexFrom == null) throw new Exception($"Vertex with name \"{from}\" not exist.");
            Vertex vertexTo = FindVertex(to);
            if (vertexTo == null) throw new Exception($"Vertex with name \"{to}\" not exist.");
            if (vertexFrom.IsLinkExists(from, to)) throw new Exception($"Link from \"{from}\" to \"{to}\" already exist.");

            Link link = new Link(LinkId++, title, weight, vertexFrom, vertexTo, FigureWidth, FigureHeight);

            vertexFrom.AddLink(link);
            vertexTo.AddLink(link);
            LinkCount++;
            toolTip.ChangeToolTip(this);
        } // AddLink

        public Link AddLink(string title, int weight, Vertex v1, Vertex v2)
        {
            Link link = new Link(LinkId++, title, weight, v1, v2, FigureWidth, FigureHeight);
            v1.AddLink(link);
            v2.AddLink(link);
            LinkCount++;
            toolTip.ChangeToolTip(this);
            return link;
        } // AddLink

        public Vertex FindVertex(string title) => Vertices.Find(x => x.Title == title);

        public Vertex FindVertex(int id) => Vertices.Find(x => x.Id == id);

        public bool Exist(string title) => Vertices.Exists(x => x.Title == title);
        public void RemoveVertex(Vertex vertex)
        {
            if (vertex.Links.Count > 0)
            {
                LinkCount -= vertex.Links.Count;
                RemoveLinks(vertex); // remove all links related to this Vertex
            }
            Vertices.Remove(vertex);
            toolTip.ChangeToolTip(this);
        } // RemoveVertex

        public void RemoveVertex(string title)
        {
            Vertex temp = FindVertex(title);

            if (temp.Links.Count > 0)
            {
                LinkCount -= temp.Links.Count;
                RemoveLinks(temp); // remove all links related to this Vertex
            }
            Vertices.Remove(temp);
            toolTip.ChangeToolTip(this);
        } // RemoveVertex
        public void RemoveVertex(int id)
        {
            Vertex temp = FindVertex(id);

            if (temp.Links.Count > 0)
            {
                LinkCount -= temp.Links.Count;
                RemoveLinks(temp); // remove all links related to this Vertex
            }
            Vertices.Remove(temp);
            toolTip.ChangeToolTip(this);
        } // RemoveVertexByTitle
        void RemoveLinks(Vertex vertex)
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].RemoveAllLinks(vertex); // remove all links by vertex
            toolTip.ChangeToolTip(this);
        } // RemoveLinks

        public void RemoveLink(Link link)
        {
            link.From.RemoveLink(link);
            link.To.RemoveLink(link);
            LinkCount--;
            toolTip.ChangeToolTip(this);
        }

        public void Save(string fileName)
        {
            XmlWriter writer;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.ASCII;
            settings.NewLineOnAttributes = false;

            writer = XmlWriter.Create(fileName, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Graph");
            writer.WriteAttributeString("Title", Title);
            writer.WriteAttributeString("VertexId", VertexId.ToString());
            writer.WriteAttributeString("LinkId", LinkId.ToString());
            writer.WriteAttributeString("PointX", GraphPoint.X.ToString());
            writer.WriteAttributeString("PointY", GraphPoint.Y.ToString());

            foreach (var vertex in Vertices)
                vertex.Save(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        } // Save

        public void Load(string fileName)
        {
            Vertices.Clear();
            List<TempLink> tempLinks = new List<TempLink>();

            XmlReader reader;
            reader = XmlReader.Create(fileName);

            while (reader.Read())
            {
                if (reader.HasAttributes)
                    if (reader.Name == "Graph") LoadGraph(reader);
                    else if (reader.Name == "Vertex") LoadVertex(reader);
                    else if (reader.Name == "Links") LoadLinks(reader, tempLinks);
            } // while
            reader.Close();

            ConvertTempLinks(tempLinks);
            toolTip.ChangeToolTip(this);
        } // Load

        void GetData(XmlReader reader, out string title, out int id, out int weight)
        {
            reader.MoveToFirstAttribute();
            title = reader.Value;

            reader.MoveToNextAttribute();
            id = int.Parse(reader.Value);

            reader.MoveToNextAttribute();
            weight = int.Parse(reader.Value);
        }
        void LoadGraph(XmlReader reader)
        {
            reader.MoveToFirstAttribute();
            Title = reader.Value;

            reader.MoveToNextAttribute();
            VertexId = int.Parse(reader.Value);

            reader.MoveToNextAttribute();
            LinkId = int.Parse(reader.Value);

            reader.MoveToNextAttribute();
            double x = double.Parse(reader.Value);
            reader.MoveToNextAttribute();
            double y = double.Parse(reader.Value);
            GraphPoint = new Point(x, y);

        } // LoadGraph
        void LoadVertex(XmlReader reader)
        {
            GetData(reader, out string title, out int id, out int weight);

            reader.MoveToNextAttribute();
            double x = double.Parse(reader.Value);
            reader.MoveToNextAttribute();
            double y = double.Parse(reader.Value);

            Vertices.Add(new Vertex(id, title, weight, new Point(x, y), FigureWidth, FigureHeight));

        } // LoadVertex
        void LoadLinks(XmlReader reader, List<TempLink> tempLinks)
        {
            GetData(reader, out string title, out int id, out int weight);

            reader.MoveToNextAttribute();
            string titleFrom = reader.Value;

            reader.MoveToNextAttribute();
            string titleTo = reader.Value;

            if (IsTempLinkExists(tempLinks, titleFrom, titleTo)) return;

            reader.MoveToNextAttribute();
            double x1 = double.Parse(reader.Value);
            reader.MoveToNextAttribute();
            double y1 = double.Parse(reader.Value);

            reader.MoveToNextAttribute();
            double x2 = double.Parse(reader.Value);
            reader.MoveToNextAttribute();
            double y2 = double.Parse(reader.Value);

            tempLinks.Add(new TempLink(id, title, weight, titleFrom, titleTo, new Point(x1, y1), new Point(x2, y2)));
        } // LoadLinks

        bool IsTempLinkExists(List<TempLink> tempLinks, string from, string to) => tempLinks.Exists(x => x.From == from && x.To == to);

        void ConvertTempLinks(List<TempLink> tempLinks)
        {
            Vertex from;
            Vertex to;
            foreach (var tempLink in tempLinks)
            {
                from = FindVertex(tempLink.From);
                to = FindVertex(tempLink.To);
                Link link = tempLink.ToLink(from, to);
                from.AddLink(link);
                to.AddLink(link);
            }
            LinkCount = tempLinks.Count;
        } // ConvertTempLinks

        public Vertex this[int index]
        {
            get
            {
                if (index >= 0 && index < Vertices.Count)
                    return Vertices[index];
                else
                    throw new Exception($"Entered indexer cant be less than 0 or bigger than {Vertices.Count - 1}");
            } // get
        } // this[int n]

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Vertices.Count; i++)
                yield return Vertices[i];
        } // GetEnumerator

        public List<Vertex> Pathfinder(Vertex from, Vertex to)
        {
            Func<Vertex, Vertex, List<Vertex>> pathfinder;

                pathfinder = DijkstraSearch;
          
            return pathfinder(from, to);
        } // Pathfinder

        public List<Vertex> DijkstraSearch(Vertex from, Vertex to)
        {
            foreach (var vertex in Vertices)
                vertex.IsVisited = false;

            Dictionary<Vertex, Vertex> parentMap = new Dictionary<Vertex, Vertex>();
            PriorityQueue<Vertex, int> priorityQueue = new PriorityQueue<Vertex, int>();

            Dictionary<Vertex, int> oldWeights =  InitializeWeight(from);

            priorityQueue.Enqueue(from, from.Weight);

            Vertex current;

            while (priorityQueue.Count > 0)
            {
                current = priorityQueue.Dequeue();
                if (!current.IsVisited)
                {
                    current.IsVisited = true;
                    if (current.Equals(to)) break;
                    foreach (var link in current.Links)
                    {
                        Vertex neighbor;
                        if (current == link.To) neighbor = link.From;
                        else neighbor = link.To;

                        int newWeight = current.Weight + link.Weight;
                        int neighborWeight = neighbor.Weight;

                        if (newWeight < neighborWeight)
                        {
                            neighbor.Weight = newWeight;
                            parentMap.Add(neighbor, current);
                            int priority = newWeight;
                            priorityQueue.Enqueue(neighbor, priority);
                        } // if
                    } // foreach
                } // f (!current.IsVisited)
            } // while

            List<Vertex> path = ReconstructPath(parentMap, from, to);

            foreach (var vertex in oldWeights)
                vertex.Key.Weight = vertex.Value;

            return path;
        } // DijkstraSearch

        public List<Vertex> ReconstructPath(Dictionary<Vertex, Vertex> parentMap, Vertex from, Vertex to)
        {
            List<Vertex> path = new List<Vertex>();
            Vertex current = to;

            while (current != from)
            {
                path.Add(current);
                if (!parentMap.ContainsKey(current)) return null; // if path not exists
                current = parentMap[current];
            } // while

            path.Add(from);
            path.Reverse();
            return path;
        } // ReconstructPath

        public Dictionary<Vertex, int> InitializeWeight(Vertex from)
        {
            Dictionary<Vertex, int> oldWeights = new Dictionary<Vertex, int>();

            foreach (var vertex in Vertices)
            {
                oldWeights.Add(vertex, vertex.Weight);
                vertex.Weight = int.MaxValue;
            }

            from.Weight = 0;
            return oldWeights;
        } // InitializeWeight

        public string GetToolTip() => $"Graph name: {Title}\nVertices: {GetVerticesCount()}\nLinks: {LinkCount}";
    } // class Graph

}

