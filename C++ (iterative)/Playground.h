#pragma once
#include "Cell.h"
#include <vector>
using namespace std;
class Playground
{
public:
	Playground(int);
	virtual ~Playground();
	void Calculate();
	void Display();
private:
	int PlaygroundSize;
	vector<Cell>CellVector;
};

