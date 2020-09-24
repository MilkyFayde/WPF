using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GraphVisual
{
    public interface IToolTip
    {
        string GetToolTip();
    }

    public class GraphToolTip<T> where T : IToolTip
    {
        string tip = "";
        public ToolTip toolTip { get; private set; } = new ToolTip();

        public GraphToolTip(T obj)
        {
            ChangeToolTip(obj);
        }
        public void ChangeToolTip(T obj)
        {
            tip = obj.GetToolTip();
            toolTip.Content = tip;
        }
    }
}
