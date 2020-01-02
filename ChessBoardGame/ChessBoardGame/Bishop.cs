using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoardGame
{
    class Bishop : ChessPiece
    {
        public Bishop(Point location, bool color)
        {
            this.color = color;
            if (color == false)
            { image = Properties.Resources.BlackBishop; }
            if (color == true)
            { image = Properties.Resources.WhiteBishop; }

            this.location = location;
        }

        //copy constructor
        public Bishop(Bishop previousChessPiece)
        {
            this.color = previousChessPiece.color;
            this.location = previousChessPiece.location;
            this.possibleMoves = previousChessPiece.possibleMoves;
            this.image = previousChessPiece.image;
        }

        public override List<Point> CalculateMoves(List<ChessPiece> chessPieces)
        {
            possibleMoves.Clear();
            canMoveDiagonally(chessPieces);
            return possibleMoves;
        }

    }
}
