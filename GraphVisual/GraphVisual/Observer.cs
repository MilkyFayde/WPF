using System;
using System.Windows;

namespace GraphVisual
{
    public interface IObserver
    {
        void Move (Point point, Link link, bool from, double figureWidth, double figureHeight);
    } // interface IObserver

    public class VertexMove : IObserver
    {
        public void Move(Point point, Link link, bool from, double figureWidth, double figureHeight)
        {
            if (from)
                link.ChangePointFrom(new Point(point.X + figureWidth / 2, point.Y + figureHeight / 2));
            else
                link.ChangePointTo(new Point(point.X + figureWidth / 2, point.Y + figureHeight / 2));
        } // Move
    } // class Observer

    public abstract class Observable
    {
        Action <Point, Link, bool, double, double> observers;

        public void AddObserver(Action<Point, Link, bool, double, double> ev) => observers += ev;

        public void Notify(Point point, Link link, bool from, double figureWidth, double figureHeight) => observers?.Invoke(point, link, from, figureWidth, figureHeight);

    } // abstract class Observable
}

