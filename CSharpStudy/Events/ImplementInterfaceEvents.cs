using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStudy
{
    public interface IDrawingObject
    {
        event EventHandler ShapeChanged;
    }

    public class MyEventArgs : EventArgs
    {
        // class members
    }

    public class DrawingShape : IDrawingObject
    {
        public event EventHandler ShapeChanged;

        void ChangeShape()
        {
            // Do something here before the event...

            OnShapeChanged(new MyEventArgs(/*arguments*/));

            // or do something here after the event.
        }

        protected virtual void OnShapeChanged(MyEventArgs e)
        {
            ShapeChanged?.Invoke(this, e);
        }
    }
}
