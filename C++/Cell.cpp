#include "Cell.h"
#include <iostream>

using namespace std;

bool Cell::GetState()
{
	return IsAlive;
}

void Cell::SetState(bool inputState)
{
	IsAlive = inputState;
}

int Cell::GetLifeLenght()
{
	return LifeLenght;
}

void Cell::SetLifeLenght(int inputLifeLenght)
{
	LifeLenght = inputLifeLenght;
}

Cell::Cell(int temp,int playgroundSize)
{
	Id = temp;
	LifeLenght = 0;
	temp = rand() % 12;
	if (temp==1)
	{
		IsAlive = true;
	}
	else
	{
		IsAlive = false;
	}
	BuildNeighbourhoodSnakeStyle(playgroundSize);
	//BuildNeighbourhoodRandomBorders(playgroundSize);
}

Cell::~Cell()
{
}

void Cell::BuildNeighbourhoodSnakeStyle(int playgroundSize)
{
	if (Id == 0)                                                 //Lewy górny
	{
		NeighboursId[0] = playgroundSize*playgroundSize - 1;
		NeighboursId[1] = playgroundSize*(playgroundSize - 1);
		NeighboursId[2] = NeighboursId[1] + 1;
		NeighboursId[3] = playgroundSize - 1;
		NeighboursId[4] = Id + 1;
		NeighboursId[5] = 2 * playgroundSize - 1;
		NeighboursId[6] = playgroundSize;
		NeighboursId[7] = NeighboursId[6] + 1;
	} 
	else
	{
		if (Id == (playgroundSize*playgroundSize) - 1)               //Prawy dolny
		{
			NeighboursId[0] = Id - (playgroundSize + 1);
			NeighboursId[1] = NeighboursId[0] + 1;
			NeighboursId[2] = playgroundSize*(playgroundSize - 2);
			NeighboursId[3] = Id - 1;
			NeighboursId[4] = playgroundSize*(playgroundSize - 1);
			NeighboursId[5] = playgroundSize - 2;
			NeighboursId[6] = NeighboursId[5] + 1;
			NeighboursId[7] = 0;
		}
		else
		{
			if (Id == playgroundSize - 1)                            //Prawy górny
			{
				NeighboursId[0] = playgroundSize*playgroundSize- 2;
				NeighboursId[1] = NeighboursId[0] + 1;
				NeighboursId[2] = playgroundSize*(playgroundSize - 1);
				NeighboursId[3] = Id - 1;
				NeighboursId[4] = 0;
				NeighboursId[5] = Id+playgroundSize - 1;
				NeighboursId[6] = NeighboursId[5] + 1;
				NeighboursId[7] = playgroundSize;
			}
			else
			{
				if (Id == playgroundSize*(playgroundSize - 1))            //Lewy dolny
				{
					NeighboursId[0] = playgroundSize*(playgroundSize - 1)-1;
					NeighboursId[1] = Id-playgroundSize;
					NeighboursId[2] = NeighboursId[1] + 1;
					NeighboursId[3] = playgroundSize*playgroundSize - 1;
					NeighboursId[4] = Id + 1;
					NeighboursId[5] = playgroundSize - 1;
					NeighboursId[6] = 0;
					NeighboursId[7] = NeighboursId[6] + 1;
				}
				else                                                           
				{
					for (int i = 0; i < 8; i++)                                      
					{
						if (i < 3)
						{
							NeighboursId[i] = Id - (playgroundSize + 1 - i);
							if (Id < playgroundSize - 1)
							{
								if (i == 0)
								{
									NeighboursId[i] = (playgroundSize*playgroundSize - 1) - (playgroundSize - Id);
								}
								else
								{
									NeighboursId[i] = NeighboursId[0] + i;
								}
							}
							else
							{
								if (Id%playgroundSize == 0)
								{
									NeighboursId[0] = Id - 1;
								}
								else
								{
									if ((Id + 1) % playgroundSize == 0)
									{
										NeighboursId[2] = Id - ((2 * playgroundSize)-1);
									}
								}
							}
						}
						if (i == 3)
						{
							NeighboursId[i] = Id - 1;
							if (Id%playgroundSize == 0)
							{
								NeighboursId[i] = Id + (playgroundSize - 1);
							}
						}
						if (i == 4)
						{
							NeighboursId[i] = Id + 1;
							if ((Id + 1) % playgroundSize == 0)
							{
								NeighboursId[i] = Id - (playgroundSize - 1);
							}
						}
						if (i > 4)
						{
							NeighboursId[i] = Id + (playgroundSize -1 + (i-5));
							if (Id > playgroundSize*(playgroundSize-1))
							{
								if (i == 5)
								{
									NeighboursId[i] = Id - (playgroundSize*(playgroundSize - 1)) - 1;
								}
								else
								{
									NeighboursId[i] = NeighboursId[5] + (i-5);
								}
							}
							else
							{
								if (Id%playgroundSize == 0)
								{
									NeighboursId[5] = Id + (2 * playgroundSize) - 1;
								}
								else
								{
									if ((Id + 1) % playgroundSize == 0)
									{
										NeighboursId[7] = Id + 1;
									}
								}
							}
						}
					}
				}
			}
		}
	}
}

void Cell::BuildNeighbourhoodRandomBorders(int playgroundSize)
{
	for (int i = 0; i < 8; i++)
	{
		if (i < 3)
		{
			NeighboursId[i] = Id - (playgroundSize + 1 - i);
		}
		else
		{
			if (i == 3)
			{
				NeighboursId[i] = Id - 1;
			}
			else
			{
				if (i == 4)
				{
					NeighboursId[i] = Id + 1;
				}
				else
				{
					NeighboursId[i] = Id + (playgroundSize - 1 + (i - 5));
				}
			}
		}
	}
	if (Id < playgroundSize)
	{
		for (int i = 0; i < 3; i++)
		{
			NeighboursId[i] = -1;
		}
	}
	else 
	{
		if (Id>playgroundSize*(playgroundSize - 1))
		{
			for (int i = 5; i < 8; i++)
			{
				NeighboursId[i] = -1;
			}
		}
	}
	if (Id%playgroundSize == 0)
	{
		NeighboursId[0] = -1;
		NeighboursId[3] = -1;
		NeighboursId[5] = -1;
	}
	else
	{
		if ((Id + 1) % playgroundSize == 0)
		{
			NeighboursId[2] = -1;
			NeighboursId[4] = -1;
			NeighboursId[7] = -1;
		}
	}
}