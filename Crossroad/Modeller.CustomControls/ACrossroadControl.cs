using System.Collections.Generic;
using System.Windows.Forms;

namespace Modeller.CustomControls
{
    public abstract class ACrossroadControl : UserControl
    {
        protected IList<RoadLine> _roadLines;

        protected ACrossroadControl()
        {
            _roadLines = new List<RoadLine>();
        }

        public virtual void AddLine(RoadLine roadLine)
        {
            _roadLines.Add(roadLine);
        }

        public virtual void DeleteLine(RoadLine roadLine)
        {
            _roadLines.Remove(roadLine);
        }
    }
}