using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZero.Common.Collections
{
    public class ExpandingGrid<T> : IEnumerable<T>
    {
        //Orientation is only used by public facing methods. The public methods are responsible for converting to the orientation prior to calling private methods.
        private Orientation _orientation;
        private int _columns;
        private List<ExpandingGridRow> _rows;

        public ExpandingGrid(int columns, Orientation orientation = Orientation.Vertical)
        {
            _columns = columns;
            _orientation = orientation;
            _rows = new List<ExpandingGridRow>();
        }

        public void AddElement(T element, int colSpan, int rowSpan)
        {
            if (colSpan >= _columns)
            {
                colSpan = _columns;
            }
            var elem = new ExpandingGridElement(element, colSpan, rowSpan);
            
            for (int y = 0; y < _rows.Count; ++y)
            {
                for (int x = 0; x < _columns - colSpan + 1; ++x)
                {
                    bool canFit = ElementFits(x, y, colSpan, rowSpan);
                    if (canFit)
                    {
                        ExpandGridToFit(y, rowSpan);
                        Insert(elem, x, y);
                        return;
                    }
                }
            }

            // EDGE CASE: There are no open spots for element. Create new row and insert.
            int row = _rows.Count;
            ExpandGridToFit(row, rowSpan);
            Insert(elem, 0, row);
        }

        private void Insert(ExpandingGridElement element, int column, int row)
        {
            for (int y = 0; y < element.RowSpan; ++y)
            {
                int currentRow = y + row;
                if (currentRow >= _rows.Count)
                {
                    throw new IndexOutOfRangeException("Attempting to insert into a grid which has not been properly expanded. Something broke.");
                }

                for (int x = 0; x < element.ColumnSpan; ++x)
                {
                    int currentColumn = x + column;
                    if (currentColumn >= _columns)
                    {
                        throw new IndexOutOfRangeException("Attempting to insert into a column without enough room.");
                    }
                    if (_rows[currentRow][currentColumn] != null)
                    {
                        throw new InvalidOperationException("Attempting to insert an element over another one. Overlap is NOT permitted.");
                    }
                    _rows[currentRow][currentColumn] = element;
                }
            }
        }

        private bool ElementFits(int column, int row, int columnSpan, int rowSpan)
        {
            for (int y = 0; y < rowSpan; ++y)
            {
                if (row + y >= _rows.Count)
                {
                    return true;
                }
                
                for (int x = 0; x < columnSpan; ++x)
                {
                    if (column + x > _columns)
                    {
                        return false;
                    }

                    //If it contains an element inside where we are trying to place the element it can't be placed here.
                    if (_rows[row + y][column + x] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void ExpandGridToFit(int row, int rowSpan)
        {
            int addNumber = row + rowSpan - _rows.Count;
            for(int i = 0; i < addNumber; ++i)
            {
                _rows.Add(new ExpandingGridRow(_columns));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            LinkedList < ExpandingGridElement > pickedElements = new LinkedList<ExpandingGridElement>();
            foreach (var row in _rows)
            {
                for (int x = 0; x < _columns; ++x)
                {
                    ExpandingGridElement elementAtPosition = row[x];
                    if (elementAtPosition != null && !pickedElements.Contains(elementAtPosition))
                    {
                        pickedElements.AddFirst(elementAtPosition);
                        yield return elementAtPosition.Element;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Sub Classes

        private class ExpandingGridRow : List<ExpandingGridElement>
        {
            public ExpandingGridRow(int columns)
                : base(columns)
            {
                //Add null columns
                for(int i = 0; i < columns; ++i)
                {
                    Add(null);
                }
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
