#pragma once
class Cell
{
public:
	int NeighboursId[8];
	bool GetState();
	void SetState(bool);
	int GetLifeLenght();
	void SetLifeLenght(int);
	Cell(int,int);
	virtual ~Cell();
private:
	int LifeLenght;
	int Id;
	bool IsAlive;
	void BuildNeighbourhoodSnakeStyle(int);
	void BuildNeighbourhoodRandomBorders(int);
};