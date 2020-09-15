using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace GraphVisual
{
    public class Link
    {
        public string Title { get; private set; }
        public int Id { get; private set; }
        public int Weight { get; private set; }
        public Vertex From { get; private set; }
        public Vertex To { get; private set; }
        public Point PointFrom { get; set; }
        public Point PointTo { get; set; }

        public Link(int id, string title, int weight, Vertex from, Vertex to, double figureWidth, double figureHeight)
        {
            Id = id;
            Title = title;
            Weight = weight;
            From = from;
            To = to;

            PointFrom = new Point(from.VertexPoint.X + figureWidth / 2, from.VertexPoint.Y + figureHeight / 2);
            PointTo = new Point(to.VertexPoint.X + figureWidth / 2, to.VertexPoint.Y + figureHeight / 2);
        } // Link
        public Link(int id, string title, int weight, Vertex from, Vertex to, Point pointFrom, Point pointTo)
        {
            Id = id;
            Title = title;
            Weight = weight;
            From = from;
            To = to;

            PointFrom = pointFrom;
            PointTo = pointTo;
        } // Link
        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement("Links");
            writer.WriteAttributeString("Title", Title);
            writer.WriteAttributeString("Id", Id.ToString());
            writer.WriteAttributeString("Weight", Weight.ToString());
            writer.WriteAttributeString("From", From.Title);
            writer.WriteAttributeString("To", To.Title);
            writer.WriteAttributeString("PointFromX", PointFrom.X.ToString());
            writer.WriteAttributeString("PointFromY", PointFrom.Y.ToString());
            writer.WriteAttributeString("PointToX", PointTo.X.ToString());
            writer.WriteAttributeString("PointToY", PointTo.Y.ToString());
            writer.WriteEndElement();
        } // Save

        public void ChangePointFrom(Point point)
        {
            PointFrom = point;
        }
        public void ChangePointTo(Point point)
        {
            PointTo = point;
        }
    } // class Link

    public class TempLink
    {
        public string Title { get; private set; }
        public int Id { get; private set; }
        public int Weight { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public Point PointFrom { get; private set; }
        public Point PointTo { get; private set; }
        public TempLink(int id, string title, int weight, string from, string to, Point pointFrom, Point pointTo)
        {
            Id = id;
            Title = title;
            Weight = weight;
            From = from;
            To = to;
            PointFrom = pointFrom;
            PointTo = pointTo;
        } // TempLink

        public Link ToLink(Vertex from, Vertex to) => new Link(Id, Title, Weight, from, to, PointFrom, PointTo);

    } // class TempLink
}
