#include <iostream>
#include <cstdlib>
#include "Windows.h"
#include "Playground.h"
#include "Cell.h"
using namespace std;

int main(int argc, char** argv)
{
	int TurnCounter = 0;
	Playground playground1(48);
	do
	{
		playground1.Calculate();
		playground1.Display();
	} while (!(GetAsyncKeyState(VK_UP)));
	getchar();
}