#include "stdafx.h"
#include "class_Engine_App.h"
#include "MethodsSelection.h"

#include <stdlib.h> // rand
#include <ctime>	// time
#include <set>
#include <time.h>

//----------------------------------------------------------------------------------------------------------------
bool class_Engine_App::M_load_dataf(char *f_name)
{
	return false;
}
//----------------------------------------------------------------------------------------------------------------
bool class_Engine_App::M_main_func(int N_pop, int Dec, int Sel, int Cros, int K, int n, int W, int M, int joust)
{
	this->C_Constant_parameters = class_Constant_parameters();
	
	if( (0 != N_pop) )
	{
		// устанавливаем параметры в классе
		this->M_set_N(N_pop);
		this->M_set_type_of_decoder(Dec);
		this->M_set_type_selection(Sel);
		this->M_set_type_of_crossing(Cros);
		this->M_set_K_gen(K);
		if( (0 != n) && (0 != W) && (0 != M) )
		{
			// устанавливаем константные параметры в классе
			C_Constant_parameters.M_set_all(n,W,M);				
		}			
	} 

	//генерируем данные для n прямоугольников
	create_input_rectangles();


	WriteLogFile("Algorithm start M_main_func:: \n Population count " + this->M_get_N() +
		" Decoder " + this->M_get_type_of_decoder() + " Selection " + this->M_get_type_selection() + 
		" Crossover " + this->M_get_type_of_crossing() +
		" Generation coefficient " + this->M_get_K_gen() + 
		" Rectangles count " + C_Constant_parameters.M_get_n() +
		" Max width " + C_Constant_parameters.M_get_W() + " Generation count " +
		C_Constant_parameters.M_get_M());

	// генерируем начальную популяцию			
	this->M_generation_first_population();	
	WriteLogFile("Generating first population...");	

	// creating fitness function array
	double** Ro = new double*[this->C_Constant_parameters.M_get_M()];
	for ( int i = 0; i < this->C_Constant_parameters.M_get_M(); i++ )
		Ro[i] = new double[this->M_get_N()];

	// критерии останова алгоритма:
	int Curr = 0;	//Temp variable to store the position in a child array during the crossover
	int i = 0;
	for( i = 0; i < M; ++i )		// первый (достигли M)
	{
		WriteLogFile("Generation number " + (i+1));
		//Fill the child array by 0. It is going to be rewritten.
		//memset(ChildArray, 0, sizeof(ChildArray));

		for(int j(0); j < this->N; ++j)
			{
				WriteLogFile("Priority list " + j);
				for(int k(0); k < this->C_Constant_parameters.M_get_n(); ++k)
				{					
					WriteLogFile ("  id " + this->Array[i][j][k] -> M_get_ID() +
						"  w   " + Array[i][j][k]->M_get_w()+
						"  h   " + Array[i][j][k]->M_get_h() +
						" isOrient " + Array[i][j][k]->M_get_isOrient());
				}
			}

		Family* list = new Family; // list couple
		Family* Llist = list;
		

		//устанавливаем координату x по умолчанию как (ширина полосы - ширина соответствующего прямоугольника)
		for(int j(0); j < this->N; ++j)
				for(int k(0); k < this->C_Constant_parameters.M_get_n(); ++k)
					Array[i][j][k]->M_set_x(this->C_Constant_parameters.M_get_W() - Array[i][j][k]->M_get_w());
		
		//вызов декодера
		switch (this->M_get_type_of_decoder())
		{
			case 0: BLdecoder(this->Array, this->C_Constant_parameters.M_get_n(), this->M_get_N(), i); break;
			case 1: IBLdecoder(this->Array, this->C_Constant_parameters.M_get_n(), this->M_get_N(), i); break;
			default: break;
		}
		//проверка координат
		WriteLogFile("Coordinates: ");

		for(int j(0); j < this->N; ++j)
			{
				WriteLogFile("Priority list " + j);
				for(int k(0); k < this->C_Constant_parameters.M_get_n(); ++k)
				{					
					WriteLogFile ("  x   " + this->Array[0][j][k]->M_get_x() +
							 "  y   " + this->Array[0][j][k]->M_get_y());
				}
			}

		// расчет целевой функции для i-го поколения
		WriteLogFile("Fitness Function ");
		for ( int j = 0; j < this->M_get_N(); j++ ) 
		{
			Ro[i][j] = this->target_function(Array, i, j);
			WriteLogFile(Ro[i][j].ToString());
		}

		if (i!=M-1)
		{

			// проверка текущего поколения на одинаковые приоритетные списки
			if( true == this->M_comparison_of_two_prior_lists(i, i) )			
			{
				FreeList(list);
				break;
			}
			// проверка на одинаковые приоритетные списки для k и k-1 поколения
			if( i != 0 )														
			{
				if( true == this->M_comparison_of_two_prior_lists(i, i-1) )	
				{
					FreeList(list);
					break;
				}
				
			}

			//Selection 
			WriteLogFile("Selection started... ");
			switch (this->M_get_type_selection())
			{
				case 0: SelectionType1(Ro[i], this->M_get_N(), list); break; // на выходе список ID родителей размером N 
				case 1: SelectionType2(Ro[i], this->M_get_N(), list); break; // на выходе список ID родителей размером N
				case 2:	SelectionType3(Ro[i], this->M_get_N(), list, joust);break; // на выходе список ID родителей размером N
				default: break;
			}
			while ( list != NULL )
			{
				WriteLogFile("1 parent index " +  list->mama + " 2 parent index " +  list->papa);
				list = list->next;
				
			}

			while ((Llist)&&(Curr < this->M_get_N()))
			{
				//get a pair from a selection list and perform a crossover
				WriteLogFile("Crossover on pair " + Curr + " processed, parents:");
				WriteLogFile("First:");
				for(int II(0); II < this->C_Constant_parameters.M_get_n(); ++II)
					{					
						WriteLogFile (  "  ID  " + this->Array[i][Llist->mama][II]->M_get_ID() +
										"  x   " + this->Array[i][Llist->mama][II]->M_get_x() +
										"  y   " + this->Array[i][Llist->mama][II]->M_get_y());
					}
				WriteLogFile("Second");
				for(int II(0); II < this->C_Constant_parameters.M_get_n(); ++II)
					{
						WriteLogFile (  "  ID  " + this->Array[i][Llist->papa][II]->M_get_ID() +
										"  x   " + this->Array[i][Llist->papa][II]->M_get_x() +
										"  y   " + this->Array[i][Llist->papa][II]->M_get_y());
					}

				this->M_crossoverProcess(Array[i][Llist->mama], Array[i][Llist->papa] ,this->C_Constant_parameters, Curr);

				WriteLogFile("Got a child:");
				for(int II(0); II < this->C_Constant_parameters.M_get_n(); ++II)
					{
						WriteLogFile (  "  ID  " + this->ChildArray[Curr][II]->M_get_ID() +
										"  x   " + this->ChildArray[Curr][II]->M_get_x() +
										"  y   " + this->ChildArray[Curr][II]->M_get_y());
					}
				WriteLogFile("Continue next step");
				
				Llist = Llist->next;
				Curr++;
			}
			
			// Calculation and other operations. At this point ChildArray is filled with a childs that can be included to
			// the next population.
			create_new_generation(i+1);	
		}
		Curr = 0;		
		FreeList(list); // удаление списка родителей*/
	}

	finalGeneration = i-1;
	WriteLogFile("finalGeneration was " + finalGeneration);
	

	//находим лучшее решение
	double max_fitness = Ro[finalGeneration][0];
	int bestIdx = 0;
	for (int priorIdx = 0; priorIdx < N; priorIdx++)
	{
		if (Ro[finalGeneration][priorIdx] >= max_fitness)
		{
			max_fitness = Ro[finalGeneration][priorIdx];
			bestIdx = priorIdx;
		}
	}
	bestPriorityListIndex = bestIdx;
	WriteLogFile("Best fitness " + max_fitness + "has priority list number " + bestPriorityListIndex);
	WriteLogFile("ONLY CHECK LAST GENERATION");
	for(int j(0); j < this->N; ++j)
			{
				WriteLogFile("Priority list " + j);
				for(int k(0); k < this->C_Constant_parameters.M_get_n(); ++k)
				{					
					WriteLogFile ("  id " + this->Array[finalGeneration][j][k] -> M_get_ID() +
						"  x   " + Array[finalGeneration][j][k]->M_get_x()+
						"  y   " + Array[finalGeneration][j][k]->M_get_y() +
						" isOrient " + Array[finalGeneration][j][k]->M_get_isOrient());
				}
			}
	for ( int i = 0 ; i < this->C_Constant_parameters.M_get_M(); i++ )
		delete[] Ro[i];
	delete[] Ro;

	return false;	
}
/* K_gen - процент лучших потомков*/
void class_Engine_App::create_new_generation(int new_generation_number)
{
	int childsCount = (M_get_N()*M_get_K_gen())/100;
	WriteLogFile("childsCount in new gen : " + childsCount);
	class_Rectangle **** children = new class_Rectangle***[1];
	children[0] = new class_Rectangle**;
	children[0] = ChildArray;
	double* children_fitness = new double[M_get_N()]; 
	double* sorted_children_fitness = new double[M_get_N()]; 

	//устанавливаем координату x по умолчанию как (ширина полосы - ширина соответствующего прямоугольника)
	for(int j(0); j < M_get_N(); ++j)
				for(int k(0); k < C_Constant_parameters.M_get_n(); ++k)
					children[0][j][k]->M_set_x(C_Constant_parameters.M_get_W() - children[0][j][k]->M_get_w());

	//вызов декодера
		switch (this->M_get_type_of_decoder())
		{
			case 0: BLdecoder(children, C_Constant_parameters.M_get_n(),M_get_N(), 0); break;
			case 1: IBLdecoder(children, C_Constant_parameters.M_get_n(), M_get_N(), 0); break;
			default: break;
		}
	//проверка координат
		WriteLogFile("Children Coordinates: ");

		for(int j(0); j < this->N; ++j)
			{
				WriteLogFile("Priority list " + j);
				for(int k(0); k < this->C_Constant_parameters.M_get_n(); ++k)
				{					
					WriteLogFile ("ID " + children[0][j][k]->M_get_ID() + "  x   " + children[0][j][k]->M_get_x() +
							 "  y   " + children[0][j][k]->M_get_y());
				}
			}

		for ( int j = 0; j < this->M_get_N(); j++ ) 
		{
			children_fitness[j] = this->target_function(children, 0, j);
		}
		WriteLogFile("Children Fitness Function ");
		for ( int j = 0; j < M_get_N(); j++ ) 
		{
			WriteLogFile(children_fitness[j].ToString());
		}
		double maxFit = 0;
		int maxFitIdx = 0;
		set<int> myset;	
		for (int ch = 0; ch < childsCount; ch ++)
		{
			WriteLogFile("getting " + ch + " best child ");
			maxFit = 0;
			for (int i  = 0; i < M_get_N(); i++ )
			{
				if (children_fitness[i] > maxFit)
				{
					maxFit = children_fitness[i];
					maxFitIdx = i;
				}	
			}
			if (myset.find(maxFitIdx)==myset.end())
			{
				myset.insert(maxFitIdx);
				children_fitness[maxFitIdx] = -1;
				WriteLogFile("added child # " + maxFitIdx);
			}
		}
		set<int>::iterator it;
		int u = 0;
		for ( it=myset.begin() ; it != myset.end(); it++ )
		{
			WriteLogFile("get child # " + *it);
			class_Rectangle *ptr_class_Rectangle;
			for( int i_var(0); i_var < this->C_Constant_parameters.M_get_n(); ++i_var )
			{
				this->Array[new_generation_number][u][i_var] = new class_Rectangle();
				//
				long iID = this->ChildArray[*it][i_var]->M_get_ID();
				int ix = this->ChildArray[*it][i_var]->M_get_x();
				int iy = this->ChildArray[*it][i_var]->M_get_y();
				int iw = this->ChildArray[*it][i_var]->M_get_w();
				int ih = this->ChildArray[*it][i_var]->M_get_h();
				bool iisOrient = this->ChildArray[*it][i_var]->M_get_isOrient();
				//
				this->Array[new_generation_number][u][i_var]->M_set_all(iID,ix,iy,iw,ih,iisOrient);
			}
			++u;
		}
		for (int j = childsCount; j < M_get_N(); j++)
		{
			for( int i_var(0); i_var < this->C_Constant_parameters.M_get_n(); ++i_var )
			{
				this->Array[new_generation_number][j][i_var] = Array[new_generation_number-1][j][i_var];
			}
		}
		children[0] = NULL;
		delete children[0];
		delete children;
		delete []children_fitness;
		delete []sorted_children_fitness;
}


//----------------------------------------------------------------------------------------------------------------
bool class_Engine_App::M_comparison_of_two_prior_lists(int M_gen, int list1, int list2)
{
	/** функция сравнивает 2 приоритетных списка из одного поколения
		return false - списки не равны
		return true	 - списки равны
	*/
	for( int i(0); i < this->C_Constant_parameters.M_get_n(); ++i )
	{
		if( this->Array[M_gen][list1][i]->M_get_ID() != this->Array[M_gen][list2][i]->M_get_ID() ) 
			return false;		
	}
	//
	return true;
}
//----------------------------------------------------------------------------------------------------------------
bool class_Engine_App::M_comparison_of_two_prior_lists(int M_gen1, int M_gen2)
{
	/** функция сравнивает 2 приоритетных списка (можно из разных поколений)
		и устанавливает значения полей "критерии останова"(f_M,f_N) в class_Engine
	*/
	if(-1 != this->f_M[0]) this->f_M[0] = -1;	// -1 undefined
	if(-1 != this->f_M[1]) this->f_M[1] = -1;	// -1 undefined
	//
	if(-1 != this->f_N[0]) this->f_N[0] = -1;	// -1 undefined
	if(-1 != this->f_N[1]) this->f_N[1] = -1;	// -1 undefined
	//
	int j;										// для for
	int fail;									// для сравнения списков
	//
	if( M_gen1 == M_gen2 )						// производим поиск только в текущем поколении
	{
		for( int i(0); i < this->N-1; ++i )		// проверим N-1 список в поколении M_gen1
		{
			j = i;
			for( ; j < this->N-1; ++j )			// N-1, т.к. j+1
			{
				if( true == this->M_comparison_of_two_prior_lists(M_gen1, i, j+1) )
				{
					// списки равны!
					this->f_M[0] = M_gen1;
					this->f_N[0] = i;
					this->f_N[1] = j+1;
					
					// останавливаем все циклы
					i = this->N;
					j = this->N;

					// останавливаем проверку полностью
					return true;
				}
			}
		}	// for i
	}	// if
	else										// производим поиск в разных поколениях
	{	
		fail = 0;
		for( int i(0); i < this->N; ++i )		// списки поколения M_gen1
		{
			for( j = 0; j < this->N; ++j )		// списки поколения M_gen2
			{
				for( int k(0); k < this->C_Constant_parameters.M_get_n(); ++k )
				{
					if( this->Array[M_gen1][i][k]->M_get_ID() != this->Array[M_gen2][j][k]->M_get_ID() )
					{
						// останавливаем проверку
						fail++;
						k = this->C_Constant_parameters.M_get_n();

						// установим в каких местах произошли ошибки
						this->f_M[0] = M_gen1;
						this->f_M[1] = M_gen2;
						this->f_N[0] = i;
						this->f_N[1] = j;
					}
				}
			}
		}	// for
		if( this->N == fail )				// списки равны!
		{
			// останавливаем проверку полностью
			return true;
		}
	}	// else
	//
	return false;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_generation_first_population()
{
	class_Rectangle * ptr_class_Rectangle;

	// выделяем память под M поколений
	this->Array = new class_Rectangle ***[C_Constant_parameters.M_get_M()]; 

	//Allocate memory for the array of childs.

	this->ChildArray = new class_Rectangle **[this->M_get_N()];
	for (int i(0); i < this->M_get_N(); i++)
	{
		this->ChildArray[i] = new class_Rectangle* [C_Constant_parameters.M_get_n()];
		for (int j(0); j < C_Constant_parameters.M_get_n(); j++)
		{
			this->ChildArray[i][j] = new class_Rectangle;
		}
	}

	
	// выделяем память под N приоритетных списков
	for(int i(0); i < C_Constant_parameters.M_get_M(); ++i)
		this->Array[i] = new class_Rectangle **[this->M_get_N()];

	// выделяем память под n прямоугольников
	for(int i(0); i < C_Constant_parameters.M_get_M(); ++i)
		for(int j(0); j < this->M_get_N(); ++j)
			this->Array[i][j] = new class_Rectangle *[C_Constant_parameters.M_get_n()];
	
	// создаем приоритетные списки
	bool exit_w;
	long step_ID = ( this->M_get_ID()-1 );		// получаем последний созданный ID
	long temp_ID = step_ID;
	long *temp_array_gen_ID = new long [temp_ID];		// создаем массив ID
	bool *temp_array_cmp_ID = new bool [temp_ID];		// массив сравнений сгенериванных ID [x][0][x][x][0]
	for( int i(0); i < ( this->M_get_N() ); ++i )	
	{
			exit_w = true;	
			// генерация случайной последовательности ID
			for( int z(0); z < temp_ID; ++z )							// в цикле случайно заносим любой ID
				temp_array_gen_ID[z] = rand()%temp_ID+1;
			while(exit_w)
			{
				for( int z(0); z < temp_ID; ++z ) temp_array_cmp_ID[z] = false;
				exit_w = false;
				for( int z(0); z < temp_ID; ++z )
				{
					if( false == temp_array_cmp_ID[ temp_array_gen_ID[z]-1 ] ) 
						temp_array_cmp_ID[ temp_array_gen_ID[z]-1 ] = true;
					else // генерируем новое значение
					{
						temp_array_gen_ID[z] = rand()%temp_ID+1;
						exit_w = true;
					}
				}
			}

			// создание нового списка
			for(int k(0); k < C_Constant_parameters.M_get_n(); ++k)
			{
				// выделяем память под прямоугольник
				for (int created_rect_idx = 0;created_rect_idx < C_Constant_parameters.M_get_n(); created_rect_idx++ )
				{
					if (input_rectangles[created_rect_idx]->M_get_ID() == temp_array_gen_ID[k] )
					{
						ptr_class_Rectangle = new class_Rectangle(temp_array_gen_ID[k],input_rectangles[created_rect_idx]->M_get_w(), input_rectangles[created_rect_idx]->M_get_h(), input_rectangles[created_rect_idx]->M_get_isOrient() );	
						this->Array[0][i][k] = ptr_class_Rectangle;
					}				
				}
				
			}

	}

	//free memory
	for (int idx = 0; idx<C_Constant_parameters.M_get_n();idx++)
	{
		delete input_rectangles[idx];
	}
	delete []input_rectangles;

	delete [] temp_array_gen_ID;
	delete [] temp_array_cmp_ID;
	//
	return 0;
}

void class_Engine_App::create_input_rectangles ()
{
	class_Rectangle * ptr_class_Rectangle;
	input_rectangles = new class_Rectangle*[C_Constant_parameters.M_get_n()];
	// создание нового списка
	for(int k(0); k < C_Constant_parameters.M_get_n(); ++k)
	{
		int gen_w = GEN_W_MAX;
		int gen_h = GEN_H_MAX;
		int gen_isOrient = 2;
				// проверка на выход ширины прямоугольника(ов) за макс. возможную ширину поля
				if( gen_w > this->C_Constant_parameters.M_get_W() ) 
					gen_w = this->C_Constant_parameters.M_get_W();

				bool isOrient;
				// определяем можно ли вращать прямоугольник
				if(0 == rand()%gen_isOrient) isOrient = false;
				else isOrient = true;
				// выделяем память под прямоугольник
				ptr_class_Rectangle = new class_Rectangle(this->M_get_ID(),rand()%gen_w+1, rand()%gen_h+1, isOrient);
				input_rectangles[k] = ptr_class_Rectangle;
				ptr_class_Rectangle = NULL;
				// инкрементируем ID
				this->M_up_ID();
			}
}



												// декодеры:
void class_Engine_App::BLleft(class_Rectangle ****m, int i, int j, int k, bool *flag, int *old_y)
{
	int max_x=0;
	bool f=0;
	for (int l=0; l<k; l++)	//перебираем все прямоугольники, которые уже лежат
	{
		if ((m[i][j][k]->M_get_y()<(m[i][j][l]->M_get_y()+(m[i][j][l]->M_get_h())))&&((m[i][j][k]->M_get_y()+(m[i][j][k]->M_get_h()))>m[i][j][l]->M_get_y())&&(m[i][j][k]->M_get_x()>=(m[i][j][l]->M_get_x()+(m[i][j][l]->M_get_w()))))	//отбираем прямоугольники, которые лежат левее нашего
			if (max_x<=(m[i][j][l]->M_get_x()+(m[i][j][l]->M_get_w())))	//выбираем самую левую координату
			{
				max_x=m[i][j][l]->M_get_x()+(m[i][j][l]->M_get_w());
				f=1;
			}
	}
	if (f==0)
	{
		m[i][j][k]->M_set_x(0);
	}
	else
	{
		m[i][j][k]->M_set_x(max_x);
	}
	*flag=1;
	BLbottom(m, i, j, k, flag, old_y);	//переходим на движение вниз
}
//----------------------------------------------------------------------------------------------------------------
void class_Engine_App::BLbottom(class_Rectangle ****m, int i, int j, int k, bool *flag, int *old_y)
{
	int max_y=0;	//флаг
	int y;		//вспомогательная переменная
	for (int l=0; l<k; l++)	//перебираем все прямоугольники, которые уже лежат
	{
		if ((m[i][j][k]->M_get_x()<(m[i][j][l]->M_get_x()+(m[i][j][l]->M_get_w())))&&((m[i][j][k]->M_get_x()+(m[i][j][k]->M_get_w()))>m[i][j][l]->M_get_x())&&((m[i][j][k]->M_get_y()+(m[i][j][k]->M_get_h()))>m[i][j][l]->M_get_y()))	//отбираем прямоугольники, на которые может попасть наш
			if (max_y<=(m[i][j][l]->M_get_y()+(m[i][j][l]->M_get_h())))	//
				max_y=m[i][j][l]->M_get_y()+(m[i][j][l]->M_get_h());
	}
	m[i][j][k]->M_set_y(max_y);	//присваиваем новое значение
	y=m[i][j][k]->M_get_y();		//запоминаем текущую координату y

	if ((*old_y!=m[i][j][k]->M_get_y())||(*flag==0))	//если старое значение не равно новому, то есть движение вниз было, то идем на движение влево 
	{
		*old_y=y;		//запоминаем y
		BLleft(m, i, j, k, flag, old_y);	//идем на движение влево
	}
}
//----------------------------------------------------------------------------------------------------------------
void class_Engine_App::BLdecoder(class_Rectangle ****ch, int n, int m, int i)
{
	for(int j=0; j<m; j++)
	{
		bool flag;
		int old_y;
		ch[i][j][0]->M_set_x(0);	//ставим первый прямоугольник в левый нижний угол
		ch[i][j][0]->M_set_y(0);
		for(int k=1; k<n; k++)	//проходим по всем генам в хромосоме
		{
			flag=0;
			old_y=32000;
			BLbottom(ch, i, j, k, &flag, &old_y);
		}
	}
}
//----------------------------------------------------------------------------------------------------------------
void class_Engine_App::IBLleft(class_Rectangle ****m, int i, int j, int k, bool *flag, int g, int *old_y, int *old_x, int *x)
{
	bool f=0;
	int max_x=0;
	for(int l=0; l<k; l++)	//проходим по всем лежащим прямоугольникам
		if((m[i][j][k]->M_get_y()<(m[i][j][l]->M_get_y()+(m[i][j][l]->M_get_h())))&&((m[i][j][k]->M_get_y()+(m[i][j][k]->M_get_h()))>m[i][j][l]->M_get_y())&&(m[i][j][k]->M_get_x()>=(m[i][j][l]->M_get_x()+(m[i][j][l]->M_get_w()))))	//выбираем из них те, которые левее
			if(max_x<=(m[i][j][l]->M_get_x()+(m[i][j][l]->M_get_w())))	//выбираем из них самый правый
			{
				max_x=m[i][j][l]->M_get_x()+(m[i][j][l]->M_get_w());
				f=1;
			}
	if (f==0)
	{
		if ((m[i][j][g]->M_get_x()-(m[i][j][k]->M_get_w()))<=0)
		{
			m[i][j][k]->M_set_x(0);
		}
		else
		{
			m[i][j][k]->M_set_x(m[i][j][g]->M_get_x()-(m[i][j][k]->M_get_w()));
		}
	}
	else
	{
		if ((m[i][j][g]->M_get_x()-(m[i][j][k]->M_get_w()))<=max_x)
		{
			m[i][j][k]->M_set_x(max_x);
		}
		else
		{
			m[i][j][k]->M_set_x(m[i][j][g]->M_get_x()-(m[i][j][k]->M_get_w()));
		}
	}
	*x=m[i][j][k]->M_get_x();		//запоминаем текущую координату x
	*flag=1;
	IBLbottom(m, i, j, k, flag, &g, old_y, old_x, *x);	//идем на движение вниз
}
//----------------------------------------------------------------------------------------------------------------
void class_Engine_App::IBLbottom(class_Rectangle ****m, int i, int j, int k, bool *flag, int *g, int *old_y, int *old_x, int x)
{
	int max_y=0;
	int y;
	for (int l=0; l<k;l++)	//проходим по всем лежащим прямоугольникам
	{
		if ((m[i][j][k]->M_get_x()<(m[i][j][l]->M_get_x()+(m[i][j][l]->M_get_w())))&&((m[i][j][k]->M_get_x()+(m[i][j][k]->M_get_w()))>m[i][j][l]->M_get_x())&&((m[i][j][k]->M_get_y()+(m[i][j][k]->M_get_h()))>m[i][j][l]->M_get_y()))	//выбираем из них те, на которые может упасть наш
			if (max_y<=(m[i][j][l]->M_get_y()+(m[i][j][l]->M_get_h())))	//
			{
				max_y=m[i][j][l]->M_get_y()+(m[i][j][l]->M_get_h());
				*g=l;	//запоминаем на какой прямоугольник мы поставили
			}
	}
	m[i][j][k]->M_set_y(max_y);	//присваиваем новое значение
	y=m[i][j][k]->M_get_y();		//запоминаем текущую координату y

	if ((*old_y!=m[i][j][k]->M_get_y())||(*flag==0)||(*old_x!=m[i][j][k]->M_get_x()))	//если старое значение не равно новому, то есть движение вниз было, то идем на движение влево 
	{
		*old_y=y;		//запоминаем y
		*old_x=x;		//запоминаем x
		IBLleft(m, i, j, k, flag, *g, old_y, old_x, &x);	//идем на движение влево
	}
}
//----------------------------------------------------------------------------------------------------------------
void class_Engine_App::IBLdecoder(class_Rectangle ****ch, int n, int m, int i)
{
	for(int j=0; j<m; j++)
	{
		bool flag;
		int g,x;
		int old_y, old_x;
		ch[i][j][0]->M_set_x(0);		//первый прямоугольник ставим в левый нижний угол
		ch[i][j][0]->M_set_y(0);
		for(int k=1; k<n; k++)	//проходим по всем генам в хромосоме
		{
			flag=0;
			g=0;
			x=0;
			old_x=ch[i][j][k]->M_get_x();
			old_y=32000;
			IBLbottom(ch, i, j, k, &flag,&g, &old_y, &old_x, x);	//идем на движение вниз
		}
	}
}
//-----------------------------------------------------------------------------------------------------------------
double class_Engine_App::target_function(class_Rectangle **** Array, int i, int j)
{
	int square=0; 
	int total_square;
	int max_h=0;
	int max_w=0;
	
	for (int k=0; k<C_Constant_parameters.M_get_n(); k++)
		square = square + Array[i][j][k]->M_get_w()*Array[i][j][k]->M_get_h();
		
	for (int k=0; k<C_Constant_parameters.M_get_n(); k++)
		if (max_h<(Array[i][j][k]->M_get_y()+Array[i][j][k]->M_get_h()))
			max_h=Array[i][j][k]->M_get_y()+Array[i][j][k]->M_get_h();

	for (int k=0; k<C_Constant_parameters.M_get_n(); k++)
		if (max_w<(Array[i][j][k]->M_get_x()+Array[i][j][k]->M_get_w()))
			max_w=Array[i][j][k]->M_get_x()+Array[i][j][k]->M_get_w();
	
	if (max_w<C_Constant_parameters.M_get_W())
		total_square=max_h*max_w;
	else
		total_square=max_h*C_Constant_parameters.M_get_W();
	
	return (double)square/total_square;
}

//------------------------------------------------------------------------------------------------------------------
												// получение закрытых членов класса:

int class_Engine_App::M_get_N()
{
	return N;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_get_type_of_decoder()
{
	return type_of_decoder;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_get_type_selection()
{
	return type_selection;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_get_type_of_crossing()
{
	return type_of_crossing;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_get_K_gen()
{
	return K_gen;
}
int class_Engine_App::M_getFinalGenerationNumber()
{
	return finalGeneration;
}
int class_Engine_App::M_getBestPriorityIdx()
{
	return bestPriorityListIndex;
}

//----------------------------------------------------------------------------------------------------------------
												// методы с ID:
long class_Engine_App::M_get_ID()			// получение ID
{
	return spy_ID;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_up_ID()				// увеличение ID
{
	spy_ID++;
	return 0;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_set_ID(long set_ID)	// установка ID
{
	spy_ID = set_ID;
	if(spy_ID == set_ID) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
												// установка закрытых членов класса:
int class_Engine_App::M_set_N(int set_N)
{
	N = set_N;
	if(N == set_N) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_set_type_of_decoder(int set_type_of_decoder)
{
	type_of_decoder = set_type_of_decoder;
	if(type_of_decoder == set_type_of_decoder) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_set_type_selection(int set_type_selection)
{
	type_selection = set_type_selection;
	if(type_selection == set_type_selection) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_set_type_of_crossing(int set_type_of_crossing)
{
	type_of_crossing = set_type_of_crossing;
	if(type_of_crossing == set_type_of_crossing) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_set_K_gen(int set_K_gen)
{
	K_gen = set_K_gen;
	if(K_gen == set_K_gen) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Engine_App::M_set_all(int set_N, int set_type_of_decoder, int set_type_selection, int set_type_of_crossing, int set_K_gen, long set_ID)
{
	N = set_N;
	type_of_decoder = set_type_of_decoder;
	type_selection = set_type_selection;
	type_of_crossing = set_type_of_crossing;
	K_gen = set_K_gen;
	spy_ID = set_ID;
	if(N == set_N &&
		type_of_decoder == set_type_of_decoder &&
		type_selection == set_type_selection &&
		type_of_crossing == set_type_of_crossing &&
		K_gen == set_K_gen &&
		spy_ID == set_ID) 
			return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------



/* ----------------------------------------------------------------------------------
 * Crossover & mutation block
 * ---------------------------------------------------------------------------------*/


/* function M_mutationProcessOrder()
 * Provides an rectangles order mutation.
 * Input: a chromosome for mutation, number of gens.
 * Returns nothing.
 */
void class_Engine_App::M_mutationProcessOrder(class_Rectangle ** unit, int n)
{
int k = 0, l = 0;	
class_Rectangle* tempClass = new class_Rectangle;

srand (unsigned(clock()));

/*Get random index for mutation*/
k = rand()%n; /* Gen number*/
l = rand()%n;

/* Normalization of mutation points */
if ( k == l)
{
	if (l > 0) l--;
	if (l < n) l++;
}

if (MUTATION_DEBUG >=1)
  WriteLogFile("Mutation in order of rectangles. changed " + k + " and " + l + "rectangles");
//Copy classes
*tempClass = *unit[k];
*unit[k] = *unit[l];
*unit[l] = *tempClass;

delete tempClass;

return;
}	

/* function M_mutationProcessOrient()
 * Provides a rectangle orient mutation.
 * Input: a chromosome for mutation.
 * Returns nothing.
 */
void class_Engine_App::M_mutationProcessOrient(class_Rectangle ** unit, int n)
{
	srand (unsigned(clock()));
	int k = 0;
	k = rand()%(n-1); /* Gen number*/
    
	if (MUTATION_DEBUG >= 1)
	  WriteLogFile("Orient mutation w h " + unit[k]->M_get_w() + " " + unit[k]->M_get_h());
	
	unit[k]->M_replacement_wh();
	
	if (MUTATION_DEBUG >= 2)
	  WriteLogFile("new w h " + unit[k]->M_get_w() +  " " + unit[k]->M_get_h());
	
	return;
}

/* function M_mutation()
 * Input: a chromosome for mutation.
 * Returns nothing.
 */
void class_Engine_App::M_mutation(class_Rectangle ** unit, bool mode, int n)
{
/* there is not necessary to call srand because it was already called 
   in crossover main function.*/

float v1 = (float)rand()/RAND_MAX;
float v2 = (float)rand()/RAND_MAX;

if (MUTATION_DEBUG >=1)
  WriteLogFile("Mutation with v1 " + v1 + " v2 " + v2);

if ((v1 > MUTATION_MIN_1) && (v1 < MUTATION_MAX_2) || mode)
{
	M_mutationProcessOrder(unit, n);
}
if ((v2 > MUTATION_MIN_1) && (v2 < MUTATION_MAX_2))
{
	M_mutationProcessOrient(unit, n);
}
return;
}

//--------------------------------------------------------------------------------------
/* function M_createChildOnePoint()
 * Provides an one-point crossover;
 * Input arguements: p1 and p2 - parents,n - number of gens, ch - a pointer to
 * the child array and a point if crossover,
 * Returns: nothing
 */
void class_Engine_App::M_createChildOnePoint(class_Rectangle **p1, class_Rectangle ** p2, int n, class_Rectangle*** ch, int s1)
{
int k = s1;
int i,j;

if (CROSSOVER_DEBUG >= 1)
  WriteLogFile("Crossover at one point of " + s1);

for (i = 0; i < n; i++)
  {
    *((*ch)[i]) = *p1[i];
	(*ch)[i]->M_set_x(DEFAULT_X);
	(*ch)[i]->M_set_y(DEFAULT_Y);
  }
	
for (j = 0; j < n; j++)
  for(i = s1; i < n; i++)
    if (p1[i]->M_get_ID() == p2[j]->M_get_ID())
	  {
		*((*ch)[k]) = *p1[i];
		(*ch)[k]->M_set_x(DEFAULT_X);
	    (*ch)[k]->M_set_y(DEFAULT_Y);
		k++;
		break;
	  }
return;
}

/* function M_createChildTwoPoint()
 * Provides a two-point crossover;
 * Input arguements: p1 and p2 - parents
 * Returns: child array.
 */ 
void class_Engine_App::M_createChildTwoPoint(class_Rectangle ** p1, class_Rectangle ** p2, 
											 int n, class_Rectangle*** ch, int s1, int s2)
{
int i,j,k;
bool doNotProcess = false;

if (CROSSOVER_DEBUG >= 1)
  WriteLogFile("Two point crossover between " + s1 + " and" + s2 + " points");

k = 0;

/* Copy an area of s1-s2 to the child*/
for(i = s1; i < s2; i++)
  {
	//copy classes
	*((*ch)[i]) = *p1[i];
	(*ch)[i]->M_set_x(DEFAULT_X);
	(*ch)[i]->M_set_y(DEFAULT_Y);
  }
	
for (j = 0; j < n; j++)
  {
    doNotProcess = false;
	for (i = 0; i < s1; i++)
	  {
		if (p2[j]->M_get_ID() == p1[i]->M_get_ID())
	  	  {
			if (k == s1) k = s2;
			*((*ch)[k]) = *p1[i];
			(*ch)[k]->M_set_x(DEFAULT_X);
	        (*ch)[k]->M_set_y(DEFAULT_Y);
			k++;
			doNotProcess = true;
			break;
		  }
	  }
	
	/* Go to the next iteration of searching in p2 if 
	 * the digit is found.  
	 * If no, proceed with other part of p1 */
	
	if (doNotProcess) 
	  continue;
	
	for (i = s2; i < n; i++)
	  {
		if (p2[j]->M_get_ID() == p1[i]->M_get_ID())
		  {
			if (k == s1) k = s2;
			*((*ch)[k]) = *p1[i];
			(*ch)[k]->M_set_x(DEFAULT_X);
	        (*ch)[k]->M_set_y(DEFAULT_Y);
			k++;
			break;
		  }
	  }
  }

return;
}
 
/* function crossoverProcess()
 * Provides a crossover. Depending of crossover type
 * function performs one-point or two-points crossover.
 * Input: parents, constant parametrs, current step.
 * Returns: new chromosome.
 */
void class_Engine_App::M_crossoverProcess(class_Rectangle **p1, class_Rectangle **p2, class_Constant_parameters & Uclass_Constant_parameters,int current)
{
bool NotReturn1 = false;
bool NotReturn2 = false;
int n = Uclass_Constant_parameters.M_get_n();
float v1 = 0;
int i = 0;

srand (unsigned(clock()*current));

int s1 = rand()%(n-1) + 1;
int s2 = rand()%(n-1) + 1;

if (s1 > s2) /* Check crossover points order*/
  {
	i = s1;
	s1 = s2;
	s2 = i;
  }

//Allocate memory for temp child arrays
class_Rectangle **ch1 = new class_Rectangle*[n];
for (i = 0; i < n; i++)
  {
	ch1[i] = new class_Rectangle;
  }
class_Rectangle **ch2 = new class_Rectangle*[n];
for (i = 0; i < n; i++)
  {
	ch2[i] = new class_Rectangle;
  }

//An appropriate crossover is called
if (M_get_type_of_crossing() == CROSS_TYPE_ONE)
  {
	M_createChildOnePoint(p1, p2, n, &ch1, s1);
	M_createChildOnePoint(p2, p1, n, &ch2, s1);
  }
else
  {
	M_createChildTwoPoint(p1, p2, n, &ch1, s1, s2);
	M_createChildTwoPoint(p2 ,p1 ,n, &ch2, s1, s2);

  }

if (CROSSOVER_DEBUG >= 2)
  {
	WriteLogFile("Got a child 1:");
	for(int II(0); II < this->C_Constant_parameters.M_get_n(); ++II)
	  {
		WriteLogFile (  "  ID  " + ch1[II]->M_get_ID() +
						"  x   " + ch1[II]->M_get_x() +
						"  y   " + ch1[II]->M_get_y());
	  }
	WriteLogFile("Got a child 2:");
	for(int II(0); II < this->C_Constant_parameters.M_get_n(); ++II)
	  {
		WriteLogFile (  "  ID  " + ch2[II]->M_get_ID() +
						"  x   " + ch2[II]->M_get_x() +
						"  y   " + ch2[II]->M_get_y());
	  }
  }

/* checking for differents from existing units*/

v1 = (float)rand()/RAND_MAX;
 
/*  Check for identity with parents
	Done for 1 and 2 child chromosome */
i = 0;
WriteLogFile("Checking for identity v1= "  + v1);

if ((v1 >= 0) && (v1 < 0.5))
  {	
    /* Check for differences with 1st parent */
	while ((i < n) && (ch1[i]->M_get_ID() == p1[i]->M_get_ID()))
      { 
		if (CROSSOVER_DEBUG >= 2)
	      WriteLogFile(" i=" + i + "ch1[i]->M_get_ID=" + ch1[i]->M_get_ID()+ "p1[i]->M_get_ID=" + p1[i]->M_get_ID());
		i++;  //Stupid code :)
	  }

  if (i == n)
    {   
      if (CROSSOVER_DEBUG >= 1)
	    WriteLogFile("WARNING: Identity found");
      /* If equal - should never be returned */
	  NotReturn1 = true;
    }
  else
    {
      /* Lets check for differences with 2nd parent */
	  i = 0;
	  while ((i < n) && (ch1[i]->M_get_ID() == p2[i]->M_get_ID())) 
	  { 
		  if (CROSSOVER_DEBUG >= 2)
		    WriteLogFile(" i=" + i + " ch1[i]->M_get_ID= " + ch1[i]->M_get_ID()+ " p2[i]->M_get_ID= " + p2[i]->M_get_ID());
		  i++;  
	  }
	
	  /* If this child is equal to 2nd parent, it should never be returned */
	  if (i == n) NotReturn1 = true;
    }
  }

//Do a check for 2nd child if necessary
if (NotReturn1 || ((v1 >= 0.5) && (v1 <= 1)))
  {
    NotReturn1 = true;
    i = 0;
    /* Check for differences with 1st parent */
    while ((i < n) && (ch2[i]->M_get_ID() == p1[i]->M_get_ID()))
      {
	    if (CROSSOVER_DEBUG >= 2)
	      WriteLogFile(" i= " + i + " ch2[i]->M_get_ID= " + ch2[i]->M_get_ID()+ " p1[i]->M_get_ID= " + p1[i]->M_get_ID());
	    i++;  //Stupid code :)
      }
    if (i >= n)
      {   
        /* If equal - should not ne returnea as-is*/
	    NotReturn2 = true;
      }
    else
     {
       /* Lets check for differences with 2nd parent */
	   i = 0;
	   while ((i < n) && (ch2[i]->M_get_ID() == p2[i]->M_get_ID())) 
	    { 
		  if (CROSSOVER_DEBUG >= 2)
		    WriteLogFile(" i= " + i + " ch2[i]->M_get_ID= " + ch2[i]->M_get_ID()+ " p2[i]->M_get_ID= " + p2[i]->M_get_ID());
		  i++;  
	    }
	  /* If equal - should not ne returnea as-is */
	  if (i == n) NotReturn2 = true;
  }
}

if (NotReturn1)
  {
    M_mutation(ch2, NotReturn2, n);
    //Copy to Array second child
    for (i = 0; i < n; i++)
      {
	    *ChildArray[current][i] = *ch2[i];

      }
  }
else
  {
    M_mutation(ch1, false, n);
    // Copy to Array first child
    for (i = 0; i < n; i++)
      {
	    *ChildArray[current][i] = *ch1[i];
      }
  }

/*Free memory that was taken for childs
  child classes are copied to the common array*/
delete []ch1;
delete []ch2;

return;
/*End of M_crossoverProcess */
}

//----------------------------------------------------------------------------------------------------------------
class_Engine_App* class_Engine_App::getInstance()
{
	if(!instance) instance = new class_Engine_App();
	return instance;
}
//----------------------------------------------------------------------------------------------------------------
// конструкторы:
class_Engine_App::class_Engine_App()
{
	FILE* pFile = fopen(LOG_FILE, "w");
	fclose(pFile);
	N = 0;
	type_of_decoder = 0;
	type_selection = 0;
	type_of_crossing = 0;
	K_gen = 0;
	spy_ID = 1;
	//
	Array = 0;
	ChildArray = 0;
	Ro = 0;
	finalGeneration = -1;
	bestPriorityListIndex = -1;
}

//----------------------------------------------------------------------------------------------------------------												// деструктор:
class_Engine_App::~class_Engine_App()
{
	//Free for ChildArray[]
  if (NULL != ChildArray)
	{
	  for(int i(0); i < this->M_get_N(); i++)
	  {
		for(int j(0); j < C_Constant_parameters.M_get_n(); j++)
				 delete ChildArray[i][j];
		delete [] ChildArray[i];
	  }
	  delete [] ChildArray;
	}
	ChildArray = NULL;

	// очистка Array:
	if(0 != Array) 
	{
		// удаляем память под n прямоугольников
		for(int i(0); i < C_Constant_parameters.M_get_M(); ++i)
			for(int j(0); j < this->M_get_N(); ++j)
				delete [] Array[i][j];

		// удаляем память под N приоритетных списков
		for(int i(0); i < C_Constant_parameters.M_get_M(); ++i)
			delete [] Array[i];

		delete [] Array;	// удаляем весь массив
	}
	spy_ID = 1;
	//

	if(0 != Ro) delete [] Ro;
	instance = 0;
	finalGeneration = -1;
	bestPriorityListIndex = -1;
	this->M_set_all(-1,-1,-1,-1,-1,-1);
	for ( int i = 0; i < 2; i++ )
	{
		f_M[i] = -1;			
		f_N[i] = -1;
	}

}
//----------------------------------------------------------------------------------------------------------------
//Logging
void class_Engine_App:: WriteLogFile (System::String^ szString)
{  
  FILE* pFile = fopen(LOG_FILE, "a");
  if (pFile!= NULL)
  {
	array<unsigned char>^ arr = System::Text::Encoding::GetEncoding(866)->GetBytes(szString);
	pin_ptr<unsigned char> ptr = &arr[0];
	char* str = (char*)ptr;
	fprintf(pFile, "%s\n", str);
	fclose(pFile);
  } 
}

class_Engine_App* class_Engine_App::instance = 0;