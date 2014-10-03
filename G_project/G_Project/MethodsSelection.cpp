#include "stdafx.h"
#include "MethodsSelection.h"


int SelectionType1(double* fitnessArray, int size, Family* &list)
{
	int code_error = 0;
	Family* top = list;
	Family* end = list;
	int p, m;

	p = ProportionalMethod(fitnessArray, size); // first couple
	m = ProportionalMethod(fitnessArray, size, p);
	list->papa = p;
	list->mama = m;
	list->next = NULL;
	int countCouple = size - 1;
	for ( int i = 0; i < countCouple; i++ )
	{
		p = ProportionalMethod(fitnessArray, size); 
		m = ProportionalMethod(fitnessArray, size, p);
		while ( list != NULL )
		{
			if ( p == list->papa && m == list->mama || p == list->mama && m == list->papa )
			{
				m = ProportionalMethod(fitnessArray, size, p);
				list = top;
			}
			else
			{
				list = list->next;
			}
		}
		Family* plist = new Family;
		plist->papa = p;
		plist->mama = m;
		plist->next = NULL;

		end->next = plist;
		end = end->next;
		list = top;
	}
	return code_error;
}

int SelectionType2(double* fitnessArray, int size, Family* &list)
{
	int code_error = 0;

	code_error = RankMethod (fitnessArray, size); //change a in function
	if ( code_error != 0 )
	{
		return code_error;
	}

	SelectionType1(fitnessArray, size, list);

	return code_error;
}

int SelectionType3(double* fitnessArray, int size, Family* &list, int joust)
{
	int code_error = 0;
	Family* top = list;
	Family* end = list;
	int p, m;

	p = JoustMethod(fitnessArray, size, joust); 
	m = JoustMethod(fitnessArray, size, joust, p);
	list->papa = p;
	list->mama = m;
	list->next = NULL;

	int countCouple = size - 1;
	for ( int i = 0; i < countCouple; i++ )
	{
		p = JoustMethod(fitnessArray, size, joust); 
		m = JoustMethod(fitnessArray, size, joust, p);
		while ( list != NULL )
		{
			if ( p == list->papa && m == list->mama || p == list->mama && m == list->papa )
			{
				m = JoustMethod(fitnessArray, size, joust, p); 
				list = top;
			}
			else
			{
				list = list->next;
			}
		}
		Family* plist = new Family;
		plist->papa = p;
		plist->mama = m;
		plist->next = NULL;

		end->next = plist;
		end = end->next;
		list = top;
	}

	return code_error;
}
int ProportionalMethod (double* fitnessArray, int size)
{
	srand (unsigned(clock()));
	double* b = new double[size];
	double sum = 0.0;
	double sm = 0.0;
	int selectI;

	double random = rand()%100 * 0.01 + 0.01; // from 0.01 to 1 
	//cout<<endl <<random <<endl;

	for ( int i = 0; i < size; i++ )
		sum += fitnessArray[i];

	for ( int i = 0; i < size; i++ )
	{
		b[i] = fitnessArray[i] / sum + sm / sum; // from 0 to 1: e.g. 10% + 20% + 12% ...
		sm += fitnessArray[i];
		//cout<<b[i] <<" ";
		if ( i == 0 && random >= 0 && random <= b[i] )
		{
			selectI = i;
			//cout<<i<<" "<<fitnessArray[i];
			break;
		}
		else if ( random >= b[i - 1] && random <= b[i] )
		{
			selectI = i;
			//cout<<i<<" "<<fitnessArray[i];
			break;
		}
	}
	//cout<<endl;
	delete[] b;
	b = NULL;
	return selectI;
}

int ProportionalMethod (double* fitnessArray, int size, int mama)
{
	int selectI;
	selectI = ProportionalMethod(fitnessArray, size);
	while (selectI == mama)
		selectI = ProportionalMethod(fitnessArray, size);
	return selectI;
}

int RankMethod (double* &fitnessArray, int size)
{
	srand (unsigned(clock()));
	int code_error = 0;
	double* b = new double[size * 2];

	if ( b == NULL )
	{
		delete [] b;
		b = NULL;
		return MEM_ERROR;
	}

	for ( int l = 0; l < size; l ++ )
	{
		b[l] = fitnessArray[l];
		b[size + l] = l;
	}

	sort(b, 0, size, size);


	//for ( int l = 0; l < (size * 2); l ++ )
	//{
	//	cout <<b[l] <<" ";
	//}
	//cout<< endl;

	int i = 0;
	int countI, sumI, n;

	while ( i < size )
	{
		countI = 1;
		sumI = i + 1;
		n = i + 1;
		for ( int j = i; j < (size-1); j ++ )
		{
			if ( abs(b[j] - b[ j + 1 ]) <= EPSILON )
			{
				countI += 1;
				sumI += (j + 2);
				n ++;
			}
			else
				break;
		}
		for ( int k = i; k < n; k ++ )
		{
			b[k] = (double) sumI / countI;
		}
		i = n;
	}

	//for ( int i = 0; i < (size * 2); i ++ )
	//{
	//	cout <<b[i] <<" ";
	//}
	//cout<< endl;

	for ( int l = 0; l < size; l ++ )
	{
		fitnessArray[l] = b[ (int) b[size + l] ];
		//cout<< a[l] << " ";
	}

	delete[] b;		b = NULL;
	return code_error;
}

int JoustMethod (double* fitnessArray, int size, int joust)
{
	srand (unsigned(clock()));
	int selectI;
	double* b = new double[joust]; 
	int random;
	for ( int i = 0; i < joust; i++ )
	{
		random = rand()%size; // from 0 to (N - 1) 		
		//cout<<endl <<random <<endl;
		b[i] = fitnessArray[random];
	}

	sort(b, 0, joust);

	for ( int i = 0; i < size; i++ )
	{
		if ( b[0] == fitnessArray[i] )  // select first max in joust
		{
			selectI = i;
			break;
		}
	}

	delete[] b;
	b = NULL;
	return selectI;
}

int JoustMethod (double* fitnessArray, int size, int joust, int mama)
{
	int selectI;
	selectI = JoustMethod(fitnessArray, size, joust);
	while (selectI == mama)
		selectI = JoustMethod(fitnessArray, size, joust);
	return selectI;
}

int sort(double *mas, int a, int size, int n)
{ 
	int i, j, flag;
	double x;
	double y;
	if ( a >= size )	return 0;
	for ( i = a, j = size-1, flag = 1; i < j ; flag > 0 ? j-- : i++ )
		if ( mas[i] < mas[j] )
		{
			x = mas[j];
			mas[j] = mas[i];
			mas[i] = x;
			y = mas[n + j];
			mas[n + j] = mas[n + i];
			mas[n + i] = y;

			flag   = 0;
		}
	sort(mas, a, j, n);
	sort(mas, i+1, size, n);
}

int sort(double *mas, int a, int size)
{ 
	int i, j, flag;
	double x;
	if ( a >= size )	return 0;
	for ( i = a, j = size-1, flag = 1; i < j ; flag > 0 ? j-- : i++ )
		if ( mas[i] < mas[j] )
		{
			x = mas[j];
			mas[j] = mas[i];
			mas[i] = x;
			flag   = 0;
		}
	sort(mas, a, j);
	sort(mas, i+1, size);
}


int GenerateArray(double* &fitnessArray,int size)
{
	srand (unsigned(clock()));

	int code_error = 0;

	fitnessArray = new double[size];

	if( fitnessArray == NULL )
	{
		delete [] fitnessArray;
		fitnessArray = NULL;
		return MEM_ERROR;
	}

	for (int i=0; i < size; i++)
	{
		fitnessArray[i] = rand()%100 * 0.01 + 0.01; // from 0.01 to 1 
		//cout<< a[i]<<" ";
	}

	return code_error;
}

void FreeArray( double* &fitnessArray)
{
	delete [] fitnessArray;
	fitnessArray = NULL;
}
void FreeList( Family* &list)
{
	Family* top = list;
	while ( top != NULL )
	{
		top = top->next;
		delete list; list = NULL;
		list = top;
	}
}