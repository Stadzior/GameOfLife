#include "Playground.h"
#include "Cell.h"
#include <vector>
#include <iostream>
#include <ctime>
using namespace std;

Playground::Playground(int inputSize)
{
	srand(time(NULL));
	this->PlaygroundSize = inputSize;
	CellVector.reserve(PlaygroundSize*PlaygroundSize);
	for (int i = 0; i < PlaygroundSize*PlaygroundSize; i++)
	{
		CellVector.emplace_back(i, PlaygroundSize);
	}
}

Playground::~Playground()
{
}

void Playground::Calculate(){
	int tempCounter,k;
	for (int i = 0; i < PlaygroundSize*PlaygroundSize; i++)
	{
			if (CellVector[i].GetState() == true)                           //Je¿eli ¿ywa.
			{
				tempCounter = 0;
				for (k = 0; k < 8; k++)
				{
					if (CellVector[i].NeighboursId[k] == -1)
					{
						if (rand() % 2 == 1)
						{
							tempCounter++;
						}
					}
					else
					{
						if (CellVector[CellVector[i].NeighboursId[k]].GetState() == true)
						{
							tempCounter++;
						}
					}
				}
				if ((tempCounter<2)||(tempCounter>3))
				{
					CellVector[i].SetState(false);
					CellVector[i].SetLifeLenght(0);
				}
				else
				{
					CellVector[i].SetLifeLenght(CellVector[i].GetLifeLenght()+1);
				}
			}
			else                                                               //Je¿eli martwa.
			{
				tempCounter = 0;
				for (k = 0; k < 8; k++)
				{
					if (CellVector[i].NeighboursId[k] == -1)
					{
						if (rand() % 2 == 1)
						{
							tempCounter++;
						}
					}
					else
					{
						if (CellVector[CellVector[i].NeighboursId[k]].GetState() == true)
						{
							tempCounter++;
						}
					}
				}
				if (tempCounter == 3)
				{
					CellVector[i].SetState(true);
				}
			}
			if (CellVector[i].GetLifeLenght() > 20)
			{
				CellVector[i].SetState(false);
			}
	}
}

void Playground::Display()
{
	system("cls");
	for (int i = 0; i < PlaygroundSize*PlaygroundSize;i++)
	{
			if (CellVector[i].GetState() == true)
			{
				cout << "X";
			}
			else
			{
				cout << " ";
			}
			if ((i + 1) % PlaygroundSize == 0)
			{
				cout << endl;
			}
	}
	cout << endl;
}