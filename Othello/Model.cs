﻿using System.Collections.Generic;
using System;
using System.Linq;

using Tree;

namespace OthelloModel
{
	class Position : IComparable, ICloneable
	{
		private int x, y;

		public Position(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public int getX()
		{
			return x;
		}

		public int getY()
		{
			return y;
		}

		public Position Clone()
		{
			return new Position(x, y);
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj.GetType().IsInstanceOfType(this)))
			{
				return 1;
			}

			Position otherPosition = obj as Position;
			if (x != otherPosition.x)
			{
				return x.CompareTo(otherPosition.x);
			}
			else
			{
				return y.CompareTo(otherPosition.y);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj.GetType().IsInstanceOfType(this)))
			{
				return false;
			}

			Position otherPosition = obj as Position;
			if (x == otherPosition.x && y == otherPosition.y)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		object ICloneable.Clone()
		{
			return new Position(x, y);
		}
	}

	enum Player
	{
		HUMAN,
		COMPUTER,
		UNOCCUPIED
	}

	class Move
	{
		private Position position;
		private Player whoseMove;

		public Move(Position position, Player whoseMove)
		{
			this.position = position;
			this.whoseMove = whoseMove;
		}

		public Position getPosition()
		{
			return position;
		}

		public Player getPlayer()
		{
			return whoseMove;
		}

		public Move Clone()
		{
			return new Move(position.Clone(), whoseMove);
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj.GetType().IsInstanceOfType(this)))
			{
				return 1;
			}

			Move otherMove = obj as Move;
			return position.CompareTo(otherMove.position);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj.GetType().IsInstanceOfType(this)))
			{
				return false;
			}

			Move otherMove = obj as Move;
			return (position.Equals(otherMove.position) && whoseMove == otherMove.whoseMove);
		}
	}

	class GameState
	{
		int xLength, yLength;

		private Player[,] board;
		private List<Position> humanPieces, computerPieces;

		public GameState(int xDimension, int yDimension)
		{
			xLength = xDimension;
			yLength = yDimension;

			board = new Player[xDimension, yDimension];
			humanPieces = new List<Position>();
			computerPieces = new List<Position>();

			for (int x = 0; x < xDimension; x++)
			{
				for (int y = 0; y < yDimension; y++)
				{
					board[x, y] = Player.UNOCCUPIED;
				}
			}
		}

		public GameState(GameState previousState, Move nextMove)
		{
			copyPreviousState(previousState);

			addMove(nextMove);
		}

		public GameState(GameState previousState, List<Move> nextMoves)
		{
			copyPreviousState(previousState);

			foreach (Move move in nextMoves)
			{
				addMove(move);
			}
		}

		public GameState(GameState previousState, Move[] nextMoves)
		{
			copyPreviousState(previousState);

			foreach (Move move in nextMoves)
			{
				addMove(move);
			}
		}

		private void copyPreviousState(GameState previousState)
		{
			xLength = previousState.xLength;
			yLength = previousState.yLength;

			board = new Player[xLength, yLength];
			humanPieces = previousState.humanPieces.Select(item => (Position)item.Clone()).ToList();
			computerPieces = previousState.computerPieces.Select(item => (Position)item.Clone()).ToList(); ;

			for (int x = 0; x < xLength; x++)
			{
				for (int y = 0; y < yLength; y++)
				{
					board[x, y] = previousState.board[x, y];
				}
			}
		}

		private void addMove(Move move)
		{
			board[
					move.getPosition().getX(),
					move.getPosition().getY()
				] = move.getPlayer();

			switch (move.getPlayer())
			{
				case Player.HUMAN:
					humanPieces.Add(move.getPosition());
					break;
				case Player.COMPUTER:
					computerPieces.Add(move.getPosition());
					break;
				case Player.UNOCCUPIED:
					break;
			}
		}

		public bool inBounds(Position position)
		{
			if (position.getX() < xLength
				&& position.getY() < yLength
				&& position.getX() >= 0
				&& position.getY() >= 0
				)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public Player getOccupier(Position position)
		{
			if (!inBounds(position))
			{
				throw new IndexOutOfRangeException(
					"You tried to access location ("
					+ position.getX() + ","
					+ position.getY()
					+ "). That location is not on the board."
				);
			}
			else
			{
				return board[position.getX(), position.getY()];
			}
		}

		public List<Position> allPlayerLocations(Player player)
		{
			List<Position> toReturn = null;
			switch (player)
			{
				case Player.HUMAN:
					toReturn = humanPieces.Select(item => (Position)item.Clone()).ToList();
					break;
				case Player.COMPUTER:
					toReturn = computerPieces.Select(item => (Position)item.Clone()).ToList();
					break;
				case Player.UNOCCUPIED:
					throw new NotImplementedException("The ability to get all unoccupied locations is not implemented.");
			}
			return toReturn;
		}

		public int getNumPieces(Player player)
		{
			switch (player)
			{
				case Player.HUMAN:
					return humanPieces.Count;
				case Player.COMPUTER:
					return computerPieces.Count;
				case Player.UNOCCUPIED:
				default:
					return xLength * yLength - (humanPieces.Count + computerPieces.Count);
			}
		}

		public Player this[int x, int y]
		{
			get { return board[x, y]; }
		}

		public int getXLength()
		{
			return xLength;
		}

		public int getYLength()
		{
			return yLength;
		}
	}

	static class GameLogic
	{
		public static GameState initialState()
		{
			GameState game = new GameState(8, 8);

			Move[] initialMoves = {
				new Move(new Position(3, 3), Player.HUMAN),
				new Move(new Position(4, 4), Player.HUMAN),
				new Move(new Position(3, 4), Player.COMPUTER),
				new Move(new Position(4, 3), Player.COMPUTER)
			};

			game = new GameState(game, initialMoves);

			return game;
		}

		public static List<Move> allValidMoves(GameState game, Player player)
		{
			Direction[] directions = { 
				Direction.UP, Direction.UPRIGHT, Direction.RIGHT,
				Direction.DOWNRIGHT, Direction.DOWN, Direction.DOWNLEFT,
				Direction.LEFT, Direction.UPLEFT, Direction.UP
			};

			List<Move> validMoves = new List<Move>();

			List<Position> playerPositions = game.allPlayerLocations(getOpposite(player));

			foreach (Position position in playerPositions)
			{
				foreach (Direction direction in directions)
				{
					Move potentialNewMove = new Move(getDirection(position, direction), player);
					if (isValid(game, potentialNewMove) && !validMoves.Contains(potentialNewMove))
					{
						validMoves.Add(potentialNewMove);
					}
				}
			}

			return validMoves;
		}

		enum Direction
		{
			UP,
			UPRIGHT,
			RIGHT,
			DOWNRIGHT,
			DOWN,
			DOWNLEFT,
			LEFT,
			UPLEFT
		}

		private static Position getDirection(Position position, Direction direction)
		{
			switch (direction)
			{
				case Direction.UP:
					return new Position(position.getX(), position.getY() + 1);
				case Direction.UPRIGHT:
					return new Position(position.getX() + 1, position.getY() + 1);
				case Direction.RIGHT:
					return new Position(position.getX() + 1, position.getY());
				case Direction.DOWNRIGHT:
					return new Position(position.getX() + 1, position.getY() - 1);
				case Direction.DOWN:
					return new Position(position.getX(), position.getY() - 1);
				case Direction.DOWNLEFT:
					return new Position(position.getX() - 1, position.getY() - 1);
				case Direction.LEFT:
					return new Position(position.getX() - 1, position.getY());
				case Direction.UPLEFT:
				default:
					return new Position(position.getX() - 1, position.getY() + 1);
			}
		}

		private static Player getOpposite(Player player)
		{
			return (player == Player.HUMAN) ? Player.COMPUTER : Player.HUMAN;
		}

		//returns false if it runs into an unoccupied square or the boundary
		private static bool lineToOpponent(
			GameState game,
			Position position,
			Direction direction,
			Player player,
			bool foundInterveningPiece
		) {
			if (!game.inBounds(position) || game.getOccupier(position) == Player.UNOCCUPIED)
			{
				return false;
			}
			else if (game.getOccupier(position) == player)
			{
				//you found a valid endpoint. Now it just depends on whether or not you
				//found a trapped pieces
				return foundInterveningPiece;
			}
			else
			{
				return lineToOpponent
				(
					game,
					getDirection(position, direction),
					direction,
					player,
					(game.getOccupier(position) == getOpposite(player) || foundInterveningPiece)
				);
			}
		}

		public static bool isValid(GameState game, Move move)
		{
			if (!game.inBounds(move.getPosition()) || game.getOccupier(move.getPosition()) != Player.UNOCCUPIED)
			{
				return false;
			}

			Position position = move.getPosition();
			Player player = move.getPlayer();
			if (lineToOpponent(game, getDirection(position, Direction.UP), Direction.UP, player, false))
			{
				return true;
			}
			if (lineToOpponent(game, getDirection(position, Direction.UPRIGHT), Direction.UPRIGHT, player, false))
			{
				return true;
			}
			if (lineToOpponent(game, getDirection(position, Direction.RIGHT), Direction.RIGHT, player, false))
			{
				return true;
			}
			if (lineToOpponent(game, getDirection(position, Direction.DOWNRIGHT), Direction.DOWNRIGHT, player, false))
			{
				return true;
			}
			if (lineToOpponent(game, getDirection(position, Direction.DOWN), Direction.DOWN, player, false))
			{
				return true;
			}
			if (lineToOpponent(game, getDirection(position, Direction.DOWNLEFT), Direction.DOWNLEFT, player, false))
			{
				return true;
			}
			if (lineToOpponent(game, getDirection(position, Direction.LEFT), Direction.LEFT, player, false))
			{
				return true;
			}
			if (lineToOpponent(game, getDirection(position, Direction.UPLEFT), Direction.UPLEFT, player, false))
			{
				return true;
			}

			return false;
		}

		private static List<Move> flipTiles(GameState game, Position position, Direction direction, List<Move> moveList, Player player)
		{
			if (!game.inBounds(position) || game.getOccupier(position) == Player.UNOCCUPIED)
			{
				return moveList;
			}

			if (game.getOccupier(position) == player)
			{
				//I know it's redundant, but it doesn't add much complexity
				moveList.Add(new Move(position, player));
				return moveList;
			}

			flipTiles(game, getDirection(position, direction), direction, moveList, player);
			if (moveList.Count > 0)
			{
				moveList.Add(new Move(position, player));
			}
			return moveList;
		}

		public static GameState result(GameState game, Move move)
		{
			if (isValid(game, move))
			{
				List<Move> nextMoves = new List<Move>();

				nextMoves.Add(move);

				nextMoves.AddRange(flipTiles(
					game,
					getDirection(move.getPosition(), Direction.UP),
					Direction.UP,
					new List<Move>(),
					move.getPlayer()
				));
				nextMoves.AddRange(flipTiles(
					game,
					getDirection(move.getPosition(), Direction.UPRIGHT),
					Direction.UPRIGHT,
					new List<Move>(),
					move.getPlayer()
				));
				nextMoves.AddRange(flipTiles(
					game,
					getDirection(move.getPosition(), Direction.RIGHT),
					Direction.RIGHT,
					new List<Move>(),
					move.getPlayer()
				));
				nextMoves.AddRange(flipTiles(
					game,
					getDirection(move.getPosition(), Direction.DOWNRIGHT),
					Direction.DOWNRIGHT,
					new List<Move>(),
					move.getPlayer()
				));
				nextMoves.AddRange(flipTiles(
					game,
					getDirection(move.getPosition(), Direction.DOWN),
					Direction.DOWN,
					new List<Move>(),
					move.getPlayer()
				));
				nextMoves.AddRange(flipTiles(
					game,
					getDirection(move.getPosition(), Direction.DOWNLEFT),
					Direction.DOWNLEFT,
					new List<Move>(),
					move.getPlayer()
				));
				nextMoves.AddRange(flipTiles(
					game,
					getDirection(move.getPosition(), Direction.LEFT),
					Direction.LEFT,
					new List<Move>(),
					move.getPlayer()
				));
				nextMoves.AddRange(flipTiles(
					game,
					getDirection(move.getPosition(), Direction.UPLEFT),
					Direction.UPLEFT,
					new List<Move>(),
					move.getPlayer()
				));

				return new GameState(game, nextMoves);
			}
			else
			{
				return null;
			}
		}

		public static int getScore(GameState game, Player player)
		{
			return game.getNumPieces(player);
		}

		public static int largerHumanScore(GameState state1, GameState state2)
		{
			return getScore(state1, Player.HUMAN).CompareTo(getScore(state2, Player.HUMAN));
		}

		public static int largerComputerScore(GameState state1, GameState state2)
		{
			return getScore(state1, Player.COMPUTER).CompareTo(getScore(state2, Player.COMPUTER));
		}
	}

	/*
	 * Coordinates the AI threads, the game tree, and the Controller
	*/
	class HAL9000
	{
		private Tree<Move, GameState> gameTree;

		private Queue<GameState> frameBuffer;

		public HAL9000()
		{
			gameTree = new Tree<Move, GameState>(GameLogic.initialState());

			List<Move> potentialHumanMoves = GameLogic.allValidMoves(gameTree.retrieveHead(), Player.HUMAN);
			foreach (Move move in potentialHumanMoves)
			{
				gameTree.addPath(new Path<Move>(move), GameLogic.result(gameTree.retrieveHead(), move));
			}

			frameBuffer = new Queue<GameState>();
			frameBuffer.Enqueue(gameTree.retrieveHead());
		}

		public bool makeMove(int x, int y)
		{
			Path<Move> humanMove = new Path<Move>(new Move(new Position(x, y), Player.HUMAN));
			if (gameTree.setPath(humanMove))
			{
				frameBuffer.Enqueue(gameTree.retrieveHead());

				//if computer cannot make a move, return straight back to the human
				List<Move> potentialComputerMoves = GameLogic.allValidMoves(gameTree.retrieveHead(), Player.COMPUTER);
				if (potentialComputerMoves.Count <= 0)
				{
					//don't forget to add in potential human moves
					List<Move> nextHumanMoves = GameLogic.allValidMoves(gameTree.retrieveHead(), Player.HUMAN);
					foreach (Move nextHumanMove in nextHumanMoves)
					{
						gameTree.addPath(
							new Path<Move>(nextHumanMove),
							GameLogic.result(gameTree.retrieveHead(), nextHumanMove)
						);
					}

					return true;
				}
				else
				{
					//otherwise, the computer will move until the human is able to
					List<Move> potentialHumanMoves;
					do
					{
						foreach (Move potentialComputerMove in potentialComputerMoves)
						{
							gameTree.addPath(
								new Path<Move>(potentialComputerMove),
								GameLogic.result(gameTree.retrieveHead(), potentialComputerMove)
							);
						}

						potentialComputerMoves.Sort(
							(x1, x2) =>
								GameLogic.largerComputerScore(gameTree.getPath(new Path<Move>(x1)), gameTree.getPath(new Path<Move>(x2)))
						);
						gameTree.setPath(new Path<Move>(potentialComputerMoves[0]));

						frameBuffer.Enqueue(gameTree.retrieveHead());

						potentialHumanMoves = GameLogic.allValidMoves(gameTree.retrieveHead(), Player.HUMAN);
					} while (potentialHumanMoves.Count <= 0);

					foreach (Move potentialHumanMove in potentialHumanMoves)
					{
						gameTree.addPath(
							new Path<Move>(potentialHumanMove),
							GameLogic.result(gameTree.retrieveHead(), potentialHumanMove)
						);
					}
				}

				return true;
			}
			else
			{
				return false;
			}


			//Path<Move> humanMove = new Path<Move>(new Move(new Position(x, y), Player.HUMAN));
			//if (gameTree.setPath(humanMove))
			//{
			//	frameBuffer.Enqueue(gameTree.retrieveHead());

			//	//for now, perform AI operations here

			//	makeComputerMove();


			//	//add the next possibilities for the human
			//	List<Move> potentialHumanMoves;
			//	do
			//	{
			//		potentialHumanMoves = GameLogic.allValidMoves(gameTree.retrieveHead(), Player.HUMAN);
			//		foreach (Move move in potentialHumanMoves)
			//		{
			//			gameTree.addPath(new Path<Move>(move), GameLogic.result(gameTree.retrieveHead(), move));
			//		}

			//		if (potentialHumanMoves.Count <= 0)
			//		{
			//			makeComputerMove();
			//		}
			//	} while (potentialHumanMoves.Count <= 0);
			//	return true;
			//}
			//else
			//{
			//	return false;
			//}
		}

		private void makeComputerMove()
		{
			//get all the valid moves for the computer
			List<Move> computerMoves = GameLogic.allValidMoves(gameTree.retrieveHead(), Player.COMPUTER);
			foreach (Move computerMove in computerMoves)
			{
				gameTree.addPath(new Path<Move>(computerMove), GameLogic.result(gameTree.retrieveHead(), computerMove));
			}

			if (computerMoves.Count > 0)
			{
				computerMoves.Sort(
					(x1, x2) =>
						GameLogic.largerComputerScore(gameTree.getPath(new Path<Move>(x1)), gameTree.getPath(new Path<Move>(x2)))
				);
				gameTree.setPath(new Path<Move>(computerMoves[0]));

				frameBuffer.Enqueue(gameTree.retrieveHead());
			}
		}

		public GameState currentState()
		{
			return frameBuffer.Peek();
		}

		public GameState dequeueState()
		{
			return frameBuffer.Dequeue();
		}

		public bool hasNextFrame()
		{
			return frameBuffer.Count > 0;
		}
	}
}