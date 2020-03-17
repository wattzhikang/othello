using System.Collections.Generic;
using System;
using System.Linq;

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
			xLength = previousState.xLength;
			yLength = previousState.yLength;

			board = new Player[xLength, yLength];
			humanPieces = previousState.humanPieces.Select(item => (Position) item.Clone()).ToList();
			computerPieces = previousState.computerPieces.Select(item => (Position)item.Clone()).ToList(); ;

			for (int x = 0; x < xLength; x++)
			{
				for (int y = 0; y < yLength; y++)
				{
					board[x, y] = previousState.board[x, y];
				}
			}

			addMove(nextMove);
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

		private bool inBounds(Position position)
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

			game = new GameState(game, new Move(new Position(3, 3), Player.HUMAN));
			game = new GameState(game, new Move(new Position(4, 4), Player.HUMAN));
			game = new GameState(game, new Move(new Position(3, 4), Player.COMPUTER));
			game = new GameState(game, new Move(new Position(4, 3), Player.COMPUTER));

			return game;
		}
		public static List<Move> allValidMoves(GameState game, Player player)
		{
			return null;
		}

		public static bool isValid(GameState game, Move move)
		{
			return true;
		}

		public static GameState result(GameState game, Move move)
		{
			return new GameState(game, move);
		}
	}

	class GameTree
	{
		GameState tmpHead;

		public GameTree(GameState nHead)
		{
			tmpHead = nHead;
		}

		public GameState selectPath(Move move)
		{
			if (GameLogic.isValid(tmpHead, move))
			{
				return (tmpHead = new GameState(tmpHead, move));
			}
			else
			{
				return null;
			}
		}

		public GameState getHead()
		{
			return tmpHead;
		}
	}

	class HAL9000
	{
		private GameTree game;

		public HAL9000()
		{
			game = new GameTree(GameLogic.initialState());
		}

		public bool makeMove(int x, int y)
		{
			Move move = new Move(new Position(x, y), Player.HUMAN);
			if (game.selectPath(move) != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public GameState currentState()
		{
			return game.getHead();
		}
	}
}