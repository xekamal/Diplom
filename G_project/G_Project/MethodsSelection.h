#pragma once

#include <iostream>
#include <ctime>
#include <math.h>

using namespace std;

#define MEM_ERROR 2
const double EPSILON = 0.001; 


struct Family  // list of couple
			{
				int mama;  // ID 
				int papa;  // ID
				Family* next;
			};

int SelectionType1(double* a, int size, Family* &list);
int SelectionType2(double* a, int size, Family* &list);
int SelectionType3(double* a, int size, Family* &list, int joust);

int ProportionalMethod (double* a, int size);
int ProportionalMethod (double* a, int size, int mama);
int RankMethod (double* &a, int size);
int JoustMethod (double* fitnessArray, int size, int joust);
int JoustMethod (double* fitnessArray, int size, int joust, int mama);

int sort(double *mas, int a, int size, int n); //функция сортировки массива по убыванию
int sort(double *mas, int a, int size);

int GenerateArray(double* &a, int size);
void FreeArray( double* &a);
void FreeList( Family* &list);