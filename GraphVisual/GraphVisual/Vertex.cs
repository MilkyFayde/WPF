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
    public class Vertex : Observable
    {
        public string Title { get; set; }
        public int Id { get; private set; }
        public int Weight { get; private set; }
        public List<Link> Links { get; private set; } = new List<Link>();
        public Point VertexPoint { get; private set; } // left top of Vertex on canvas
        IObserver observer = new VertexMove();
        public double FigureWidth { get; private set; }
        public double FigureHeight { get; private set; }

        public Vertex(int id, string title, int weight, Point vertexPoint, double figureWidth, double figureHeight)
        {
            Id = id;
            Title = title;
            Weight = weight;
            VertexPoint = vertexPoint;
            FigureWidth = figureWidth;
            FigureHeight = figureHeight;
            AddObserver(observer.Move);
        } // Vertex

        public bool IsLinkExists(Vertex v) =>
        Links.Exists(x => x.From == this) && Links.Exists(x => x.To == v) || Links.Exists(x => x.From == v) && Links.Exists(x => x.To == this);


        public bool IsLinkExists(string from, string to)
        {
            foreach (var link in Links)
                if (link.From.Title == from && link.To.Title == to) return true;
            return false;
        } // IsLinkExists

        public int GetLinksCount() => Links.Count();

        public bool RemoveLinkById(int linkId)
        {
            for (int i = 0; i < Links.Count; i++)
            {
                if (Links[i].Id == linkId)
                {
                    Links.RemoveAt(i);
                    return true;
                } // if
            } // for
            return false;
        } // RemoveLinkById

        public void ChangePoint(Point point)
        {
            VertexPoint = point;
            foreach (var link in Links)
                if (link.From == this) Notify(point, link, true, FigureWidth, FigureHeight);
                else Notify(point, link, false, FigureWidth, FigureHeight);
        } // ChangePoint

        public void RemoveAllLinks(Vertex vertex)
        {
            for (int i = 0; i < Links.Count; i++)
            {
                if (Links[i].From == vertex || Links[i].To == vertex)
                {
                    Links.RemoveAt(i);
                    i--;
                } // if
            } // for
        } // RemoveAllLinks

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement("Vertex");
            writer.WriteAttributeString("Title", Title);
            writer.WriteAttributeString("Id", Id.ToString());
            writer.WriteAttributeString("Weight", Weight.ToString());
            writer.WriteAttributeString("PointX", VertexPoint.X.ToString());
            writer.WriteAttributeString("PointY", VertexPoint.Y.ToString());

            foreach (var link in Links)
                link.Save(writer);

            writer.WriteEndElement();
        } // Save
    } // class Vertex
}
