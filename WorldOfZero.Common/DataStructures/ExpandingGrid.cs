using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZero.Common.DataStructures
{
    public class ExpandingGrid<T>
    {
        private Orientation mOrientation;
        private int mColumns;
        private List<ExpandingGridRow> rows;

        public ExpandingGrid(int columns, Orientation orientation = Orientation.Vertical)
        {
            mColumns = columns;
            mOrientation = orientation;
            rows = new List<ExpandingGridRow>();
        }

        public void AddElement(T element, int colSpan, int rowSpan)
        {
            var elem = new ExpandingGridElement(element, colSpan, rowSpan);
        }

        #region Sub Classes

        private class ExpandingGridRow
        {
            private ExpandingGridElement[] elements;

            public ExpandingGridRow(int columns)
            {
                elements = new ExpandingGridElement[columns];
            }
        }

        private class ExpandingGridElement
        {
            private readonly T mElement;
            private readonly int mColumnSpan;
            private readonly int mRowSpan;

            public T Element { get { return mElement; } }
            public int ColumnSpan { get { return mColumnSpan; } }
            public int RowSpan { get { return mRowSpan; } }

            public ExpandingGridElement(T element, int columnSpan, int rowSpan)
            {
                mElement = element;
                mColumnSpan = columnSpan;
                mRowSpan = rowSpan;
            }
        }

        #endregion
    }
}
