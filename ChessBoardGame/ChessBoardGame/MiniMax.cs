using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoardGame
{
    class MiniMax
    {
        private static Point empty = new Point(-1, -1);

        public MiniMax() { }


        public (int, Point, ChessPiece) CalculateOptimalMove(List<ChessPiece> chessPieces, bool playerColor, int depthRemaining, int alpha = int.MinValue, int beta = int.MaxValue, bool isBot = true)
        {
            int boardValue;
            //Base case
            if (depthRemaining <= 0)
            {
                boardValue = evaluateBoard(chessPieces, playerColor);
                return (boardValue, empty, null);
            }

            //recursion case: search for possible moves
            Point bestMove = new Point();
            List<Point> possibleMoves = new List<Point>();
            int bestMoveValue = new int();
            List<ChessPiece> tempChessPieces = new List<ChessPiece>();
            ChessPiece movingPiece = null;

            foreach (ChessPiece newChessPiece in chessPieces)
            {
                if (newChessPiece is Pawn)
                    tempChessPieces.Add(new Pawn(newChessPiece as Pawn));
                if (newChessPiece is Bishop)
                    tempChessPieces.Add(new Bishop(newChessPiece as Bishop));
                if (newChessPiece is Knight)
                    tempChessPieces.Add(new Knight(newChessPiece as Knight));
                if (newChessPiece is Rook)
                    tempChessPieces.Add(new Rook(newChessPiece as Rook));
                if (newChessPiece is Queen)
                    tempChessPieces.Add(new Queen(newChessPiece as Queen));
                if (newChessPiece is King)
                    tempChessPieces.Add(new King(newChessPiece as King));
            }

            ChessPiece bestChessPiece = null;

            bestMoveValue = isBot ? int.MinValue : int.MaxValue;

            foreach (ChessPiece chessPiece in chessPieces)
            {
                if (chessPiece.getColor() == isBot)
                {
                    foreach (ChessPiece tempChessPiece in tempChessPieces)
                    {
                        if (tempChessPiece.getLocation() == chessPiece.getLocation() && 
                            tempChessPiece.GetType() == chessPiece.GetType() && 
                            tempChessPiece.getColor() == chessPiece.getColor())
                        {
                            movingPiece = tempChessPiece;
                            possibleMoves.Clear();
                            possibleMoves.AddRange(movingPiece.CalculateMoves(tempChessPieces));
                            break;
                        }
                    }


                    foreach (Point possibleMove in possibleMoves)
                    {
                        //make a move on a temporary board which will get reset at end of loop
                        if (movingPiece != null)
                        {
                            tempChessPieces = movePiece(tempChessPieces, movingPiece, possibleMove);
                            Point temp;
                            ChessPiece tempchess;
                            boardValue = 0;
                            (boardValue, temp, tempchess) = CalculateOptimalMove(tempChessPieces, playerColor, depthRemaining - 1, alpha, beta, !isBot);

                            //         Console.WriteLine(isBot ? "max:" : "min", depthRemaining, possibleMove, boardValue, bestMove, bestMoveValue);

                            if (isBot)
                            {
                                //loke for moves that maximize position
                                if (boardValue > bestMoveValue)
                                {
                                    bestMoveValue = boardValue;
                                    bestMove = possibleMove;
                                    bestChessPiece = chessPiece;
                                }
                                alpha = Math.Max(alpha, boardValue);
                            }
                            else
                            {
                                if (boardValue < bestMoveValue)
                                {
                                    bestMoveValue = boardValue;
                                    bestMove = possibleMove;
                                    bestChessPiece = chessPiece;
                                }
                                beta = Math.Min(beta, boardValue);
                            }
                            if(beta <= alpha)
                            {
                      //          Console.WriteLine("Prune " + alpha + " " + beta);
                                break;
                            }
                            // undo changes
                            tempChessPieces.Clear();
                            foreach (ChessPiece newChessPiece in chessPieces)
                            {
                                if (newChessPiece is Pawn)
                                    tempChessPieces.Add(new Pawn(newChessPiece as Pawn));
                                if (newChessPiece is Bishop)
                                    tempChessPieces.Add(new Bishop(newChessPiece as Bishop));
                                if (newChessPiece is Knight)
                                    tempChessPieces.Add(new Knight(newChessPiece as Knight));
                                if (newChessPiece is Rook)
                                    tempChessPieces.Add(new Rook(newChessPiece as Rook));
                                if (newChessPiece is Queen)
                                    tempChessPieces.Add(new Queen(newChessPiece as Queen));
                                if (newChessPiece is King)
                                    tempChessPieces.Add(new King(newChessPiece as King));
                            }
                        }
                    }
                }
            }

          //  Console.WriteLine("Depth: " + depthRemaining + "| Best Move: " + bestMove + "| Best Move Value: " + bestMoveValue + "| ChessPiece: " + bestChessPiece);

            return (bestMoveValue, bestMove, bestChessPiece);

        }


        List<ChessPiece> movePiece(List<ChessPiece> movingChessPieces, ChessPiece movingChessPiece, Point newLocation)
        {
            if (findChessPiece(newLocation, movingChessPieces) == null)
            {
                movingChessPiece.setLocation(newLocation);
            }
            else
            {
                movingChessPieces.Remove(findChessPiece(newLocation, movingChessPieces));
                movingChessPiece.setLocation(newLocation);
            }
            return movingChessPieces;
        }

        //calculates the value of a potential board  
        int evaluateBoard(List<ChessPiece> calculatingChessPieces, bool color)
        {
            int boardScore = 0;

            foreach (ChessPiece chessPiece in calculatingChessPieces)
            {
                if (chessPiece is Pawn)
                {
                    if (chessPiece.getColor() == color)
                    {
                        boardScore = boardScore + 1;
                    }
                    else
                    {
                        boardScore = boardScore - 1;
                    }
                }
                if (chessPiece is Rook || chessPiece is Knight)
                {
                    if (chessPiece.getColor() == color)
                    {
                        boardScore = boardScore + 3;
                    }
                    else
                    {
                        boardScore = boardScore - 3;
                    }
                }
                if (chessPiece is Knight)
                {
                    if (chessPiece.getColor() == color)
                    {
                        boardScore = boardScore + 5;
                    }
                    else
                    {
                        boardScore = boardScore - 5;
                    }
                }
                if (chessPiece is Queen)
                {
                    if (chessPiece.getColor() == color)
                    {
                        boardScore = boardScore + 9;
                    }
                    else
                    {
                        boardScore = boardScore - 9;
                    }
                }
                if (chessPiece is King)
                {
                    if (chessPiece.getColor() == color)
                    {
                        boardScore = boardScore + 100000;
                    }
                    else
                    {
                        boardScore = boardScore - 100000;
                    }
                }

            }
            return boardScore;
        }



        ChessPiece findChessPiece(Point location, List<ChessPiece> findingChessPieces)
        {
            foreach (ChessPiece chessPiece in findingChessPieces)
            {
                if (chessPiece.getLocation() == location)
                    return chessPiece;
            }
            return null;
        }


    }

}
