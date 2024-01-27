using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Core.Forms.NodeControls
{
    internal class FieldTreeView : TreeView
    {
        /// <summary>
        /// Gets or sets the width (in pixels) of the first column.
        /// </summary>
        [DefaultValue( 150 )]
        public int FirstColumnWidth { get; set; } = 150;

        private SolidBrush foreColorBrush = null;
        private SolidBrush backColorBrush = null;
        private Pen foreColorDottedPen = null;
        private Pen highlightDottedPen = null;
        private Font boldFont = null;

        public FieldTreeView()
        {
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.ShowNodeToolTips = true;
        }

        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );

            if ( disposing )
            {
                this.foreColorBrush?.Dispose();
                this.backColorBrush?.Dispose();
                this.foreColorDottedPen?.Dispose();
                this.highlightDottedPen?.Dispose();
                this.boldFont?.Dispose();
            }
        }

        protected override void OnMouseUp( MouseEventArgs e )
        {
            var node = this.GetNodeAt( e.X, e.Y );

            // The treeview doesn't know the displayed width of the field nodes,
            // so we calculate the real size and check that the click is in this
            // rectangle
            if ( node != null && node is NodeField nodeField )
            {
                this.CheckPaintObjects();

                Size valueSize = TextRenderer.MeasureText( nodeField.DocumentField.Value ?? string.Empty, this.boldFont );
                Rectangle nodeSelectionRectangle = new Rectangle( nodeField.Bounds.Location, new Size( this.FirstColumnWidth + valueSize.Width, nodeField.Bounds.Height ) );

                if ( nodeSelectionRectangle.Contains( e.Location ) )
                    this.SelectedNode = nodeField;
            }

            base.OnMouseUp( e );
        }

        public void UpdateNodeTooltip( NodeField nodeField )
        {
            // Set the tooltips for nodes where the name is too long to be displayed
            Size nameSize = TextRenderer.MeasureText( nodeField.DocumentField.Name ?? string.Empty, this.Font );

            nodeField.ToolTipText = ( nameSize.Width > this.FirstColumnWidth ) ? nodeField.DocumentField.Name : string.Empty;
        }

        protected override void OnDrawNode( DrawTreeNodeEventArgs e )
        {
            if ( e.Node is NodeField node )
                this.DrawFieldNode( e, node );
            else
                e.DrawDefault = true;

            base.OnDrawNode( e );
        }

        private void DrawFieldNode( DrawTreeNodeEventArgs e, NodeField node )
        {
            // Sometimes OnDrawNode gets called with X = -1 and Y = 0 which paints all nodes in the upper left corner
            if ( e.Bounds.X == -1 )
                return;

            e.DrawDefault = false;

            string name = node.DocumentField.Name ?? string.Empty;
            string value = node.DocumentField.Value ?? string.Empty;

            if ( node.DocumentField.CustomDisplayValue != null && value != string.Empty )
                value = $"{value} ({node.DocumentField.CustomDisplayValue})";

            this.CheckPaintObjects();

            Font font = e.Node.NodeFont ?? this.Font;

            // Undo pre-drawn selection background
            e.Graphics.FillRectangle( this.backColorBrush, e.Bounds );

            bool isSelected = ( e.State & TreeNodeStates.Selected ) != 0;
            Color textColor = isSelected ? SystemColors.HighlightText : this.ForeColor;

            Size valueSize = TextRenderer.MeasureText( e.Graphics, value, this.boldFont );
            Rectangle nodeBounds = new Rectangle( e.Bounds.X, e.Bounds.Y, this.FirstColumnWidth + valueSize.Width, e.Bounds.Height );

            if ( isSelected )
            {
                // Draw selection background    
                e.Graphics.FillRectangle( SystemBrushes.Highlight, nodeBounds );
            }

            Rectangle nameBounds = e.Bounds;
            // Offset 1 pixel for the focus rectangle
            nameBounds.Offset( 1, 1 );
            nameBounds.Width = Math.Min( nameBounds.Width, this.FirstColumnWidth );

            // Draw name
            TextRenderer.DrawText( e.Graphics, name, font, nameBounds, textColor, TextFormatFlags.Left );

            // Draw value
            Point valueStartingPoint = e.Bounds.Location;
            valueStartingPoint.Offset( this.FirstColumnWidth, 1 );
            TextRenderer.DrawText( e.Graphics, value, this.boldFont, valueStartingPoint, textColor );

            // Draw dotted line between name and value
            if ( e.Bounds.Width < 150 )
                e.Graphics.DrawLine( isSelected ? this.highlightDottedPen : this.foreColorDottedPen, new Point( e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height - 3 ), new Point( e.Bounds.X + 149, e.Bounds.Y + e.Bounds.Height - 3 ) );

            // If the node has focus, draw the focus rectangle large, making
            // it large enough to include the text of the node tag, if present.
            if ( ( e.State & TreeNodeStates.Focused ) != 0 )
            {
                using ( Pen focusPen = new Pen( Color.Black ) )
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                    ControlPaint.DrawFocusRectangle( e.Graphics, nodeBounds );
                }
            }
        }

        private void CheckPaintObjects()
        {
            if ( this.foreColorBrush == null || this.foreColorBrush.Color != this.ForeColor )
            {
                this.foreColorBrush?.Dispose();

                this.foreColorBrush = new SolidBrush( this.ForeColor );
            }

            if ( this.backColorBrush == null || this.backColorBrush.Color != this.BackColor )
            {
                this.backColorBrush?.Dispose();

                this.backColorBrush = new SolidBrush( this.BackColor );
            }

            if ( this.foreColorDottedPen == null || this.foreColorDottedPen.Color != this.ForeColor )
            {
                this.foreColorDottedPen?.Dispose();

                this.foreColorDottedPen = new Pen( this.ForeColor ) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
            }

            if ( this.highlightDottedPen == null || this.highlightDottedPen.Color != SystemColors.HighlightText )
            {
                this.highlightDottedPen?.Dispose();

                this.highlightDottedPen = new Pen( SystemColors.HighlightText ) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
            }

            if ( this.boldFont == null )
            {
                this.boldFont = new Font( this.Font, FontStyle.Bold );
            }
        }

        protected override void OnFontChanged( EventArgs e )
        {
            base.OnFontChanged( e );

            this.boldFont?.Dispose();
            this.boldFont = new Font( this.Font, FontStyle.Bold );
        }
    }
}
