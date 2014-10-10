// Главный DLL-файл.
#include "StdAfx.h"
#include "lib.h"
#include <iostream>
#include <stdio.h>
#include <math.h>

#define file_education "d:/Study/Education_example.txt"

using namespace lib;
using namespace std;

Enter_data::Enter_data()
{
	Ck=0;
	Pk=0;
	stas_Pk = 0;
	Tp=0;
}

/*Traffic_Gen::Traffic_Gen(Enter_data *D)
{
//	D= new Enter_data[Neuro_crossRoad::K_svet];
}*/

void Neuron::set_axon(float a)
{
	axon = a;
}

float Neuron::get_axon()
{
	return axon;
}

bool Neuron::get_state()
{
	return state;
}

void Neuron::set_state(bool s)
{
	state = s;
}

int Neuro_crossRoad::getID()
{
	return ID;
}

Neuro_crossRoad::Neuro_crossRoad(int k_svet, int k_sost, char * file_W0,char *file_Wtek, Enter_data *Data, int CrossroadID)
{
	Wtek = file_Wtek;
	Tp_pred = 0;
	K_svet = k_svet;
	K_sost = k_sost;
	D = new Enter_data[K_svet];
	D = setData(Data, K_svet);
	S = new S_neuron*[K_svet];
	H = new H_neuron*[K_svet];
	R = new R_neuron*[K_sost];
	ID = CrossroadID;
	
	W = new float**[2];
	W[0] = new float*[K_svet];

	for (int i=0;i<K_svet;i++)
	{
		W[0][i] = new float[K_svet];
	}

	W[1] = new float*[K_svet];
	for (int i=0;i<K_svet;i++)
	{
		W[1][i] = new float[K_sost];
	}
	
	W_reader(file_W0);
/*	/////////////////////////////////////////////////////////////////////
	printf("Nachalnye W\n");
	for(int j=0;j<K_svet;j++)
			{
				for(int k=0;k<K_svet;k++)
					{
						printf("%3.3f ",W[0][j][k]);
					
					}
				printf("\n");
			}
			char c;
			scanf("%c",&c);
//////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////
	printf("\n");
	for(int j=0;j<K_svet;j++)
			{
				for(int k=0;k<K_sost;k++)
					{
						printf("%3.3f ",W[1][j][k]);
					
					}
				printf("\n");
			}
//			char c;
			scanf("%c",&c);
*//////////////////////////////////////////////////////////////////////
	for (int i=0;i<K_svet;i++)
	{
		S[i] = new S_neuron(Data[i]);
	}

	for (int i=0;i<K_svet;i++)
	{
		H[i] = new H_neuron(K_svet);
	}
	for (int i=0;i<K_sost;i++)
	{
		R[i] = new R_neuron(K_svet);
	}
	//Neuro_crossRoad_W0(file_W0);
	W_reader(file_Wtek);
}

void Neuro_crossRoad::Neuro_crossRoad_StartEducation(Enter_data * D)
{
	Neuro_crossRoad_Education_2();
	W_writer(Wtek);
	Neuro_crossRoad_Education_Kohonen(D);
}

float * Neuro_crossRoad::Neuro_crossRoad_Step( Enter_data * D)
{
	///////Передаем объекты входных данных сенсорам////////////////////
	for (int k=0;k<K_svet;k++)
	{
		S[k]->set_data(D[k]);
	}
//////////////////////////////////////////////////////////////////////////////////
/*	printf("axons S  \n");
	for(int j=0;j<K_svet;j++)
	{
		printf("%3.5f ",S[j]->get_axon());				
	}
	char c;
	scanf("%c",&c);*/
////////////////////////////////////////////////////////////////////////////////////
	////////////передача сигналов с сенсоров на скрытый слой//////////////////////
	for(int i=0; i<K_svet; i++)
	{
		for (int j=0; j< K_svet;j++)
		{
			///на вход срытого слоя подается выходное значение сенсорного нейрона
			H[i]->Dendrits[j]=S[j]->get_axon()*W[0][i][j];
		}
		H[i]->activation(K_svet);
	}
//////////////////////////////////////////////////////////////////////////////////
/*	printf("\n axons H  \n");
	for(int j=0;j<K_svet;j++)
	{
		printf("%3.5f ",H[j]->get_axon());				
	}*/
//	char c;
//	scanf("%c",&c);
////////////////////////////////////////////////////////////////////////////////////
//////самообучение сети без учителя
//	Neuro_crossRoad_Education_Hebb_S(D);
//////////////////////////////////

	for(int j=0; j<K_sost; j++)
	{
		for (int i=0; i<K_svet;i++)
		{
			///на вход нейрона выходного слоя подается выходное значение нейрона скрытого слоя
			R[j]->Dendrits[i]=H[i]->get_axon()*W[1][i][j];
			//R[j]->Dendrits[i]=D[i].Pk*W[1][i][j];
		}
		R[j]->activation(K_svet);////////////активация нейрона
	};

//////////////////////////////////////////////////////////////////////////////////
/*	printf("\n axons R  \n");
	for(int j=0;j<K_sost;j++)
	{
		printf("%3.5f ",R[j]->get_axon());				
	}
	char c;
	scanf("%c",&c);*/
////////////////////////////////////////////////////////////////////////////////////

	///////////////////Полученные значения выходного слоя записываем в массив///////////////////
	/*int * mas;
	mas = new int[K_sost];
	for (int i=0;i<K_sost;i++)
		mas[i] = R[i]->get_axon();*/
	float * mas;
	mas = new float[K_sost];
	for (int i=0;i<K_sost;i++)
		mas[i] = R[i]->get_axon();
	/*wchar_t     szText2[250];
	swprintf(szText2,  L"Res =, порты 1=%f, 2=%f, 3=%f, 4=%f", mas[0], mas[1], mas[2], mas[3]);
	MessageBox(NULL,  szText2,  L"Res",  MB_OK);*/
	return mas;
}

void Neuro_crossRoad::Neuro_crossRoad_Education_Hebb_S(Enter_data * D)
{
	float Tp_tek = 0;
//	printf("\nTp_tek = %3.2f \n",Tp_tek);				

	float a = 0.7;   /////////////// коэфициент обучения
	int k=0;
	while (k<10)
	{
		Tp_tek = 0;
		for (int i=0;i<K_svet;i++)
			Tp_tek+=D[i].Tp;

		printf("\nTp_tek = %3.5f \n",Tp_tek);
		printf("\nTp_pred = %3.5f \n",Tp_pred);
		if ((Tp_pred -Tp_tek)<0)
		{
			k++;
			for (int i=0;i<K_svet;i++)
			{
				for(int j=0;j<K_svet;j++)
				{
					W[0][i][j]+=a*S[i]->get_axon()*H[j]->get_axon();
				}
			}
/*/////////////////////////////////////////////////////////////////
			printf("\n education \n");
			for(int j=0;j<K_svet;j++)
			{
				for(int k=0;k<K_svet;k++)
					{
						printf("%3.2f ",W[0][j][k]);
					
					}
				printf("\n");
			}
			char c;
			scanf("%c",&c);
//*///////////////////////////////////////////////////////////////////
		Tp_pred=Tp_tek;
		}
		else break; 
	}
//	Tp_pred=Tp_tek;
	W_writer(Wtek);
}

void Neuro_crossRoad::Neuro_crossRoad_Education_Hebb_D(Enter_data * D)
{
	float Tp_tek = 0;
	for (int i=0;i<K_svet;i++)
		Tp_tek+=D[i].Tp;

	float a = 0.7;/////////////// коэфициент обучения

	int k=0;
	while (k<10)
	{
		if ((Tp_pred -Tp_tek)<0 && Tp_pred!=0)
		{
			k++;
			for (int i=0;i<K_svet;i++)
			{
				for(int j=0;j<K_svet;j++)
				{
					W[0][i][j]+=a*(S[i]->get_axon()-S[i]->mem_axon)*(H[j]->get_axon()-H[i]->mem_axon);
				}
			}
			Tp_pred=Tp_tek;
		}
		else break; 
	}
	Tp_pred=Tp_tek;
}

void Neuro_crossRoad::Neuro_crossRoad_Education_Kohonen(Enter_data * D)
{
	float Tp_tek = 0;
	for (int i=0;i<K_svet;i++)
		Tp_tek+=D[i].Tp;

	float a = 0.7;/////////////// коэфициент обучения

	int k=0;
	while (k<10)
	{
		if ((Tp_pred -Tp_tek)<0 && Tp_pred!=0)
		{
			k++;
			for (int i=0;i<K_svet;i++)
			{
				for(int j=0;j<K_svet;j++)
				{
					W[0][i][j]+=a*(S[i]->get_axon()-W[0][i][j]);
				}
			}
		}
		else break; 
	}
	Tp_pred=Tp_tek;
}

void Neuro_crossRoad::Neuro_crossRoad_Education_2()
{
	float *input_mas = new float[K_svet];
	float *output_mas= new float[K_sost];
	int i=0;
	float a=0.01;
	errno_t err;
	FILE * Education_example;
	err = fopen_s(&Education_example, file_education, "rt");
	float l=0;

	while(!feof(Education_example) )
	{
		i=0;		
		///////Считываем из файла строку с обучающим примером////////////////
		while (i< K_svet+K_sost)
		{
			if (i<K_svet)
			{
				fscanf(Education_example,"%f", &l);
				input_mas[i]=l;
				i++;
			}
			if (i >= K_svet)
			{
				fscanf_s(Education_example,"%f", &l);
				output_mas[i-K_svet]=l;
				i++;
			}
		}
/*		printf("From file \n");
		for (int j=0;j<K_svet;j++)
		{
			printf("%f ",input_mas[j]);
		}
		char c;
		scanf("%c",&c);

		printf("From file \n");
		for (int j=0;j<K_sost;j++)
		{
			printf("%f ",output_mas[j]);
		}
		scanf("%c",&c);
		*/

///////подаем на вход данные обучающего примера		
		for (int k=0;k<K_sost;k++)
		{
			for(int j=0;j<K_svet;j++)
			{
				R[k]->Dendrits[j]= input_mas[j]*W[1][j][k];
//				printf("W[1][%d][%d]=%0.3f\n",j,k,W[1][j][k]);
//				printf("Dendrits[%d]=%0.3f\n",j,R[k]->Dendrits[j]);
			}
			R[k]->activation(K_svet);

/////////////////////////////////////////////////////////////////////////
/*			printf("R[%d]->get_axon() =%f  ",k,R[k]->get_axon());
			printf("output_mas[%d] = %f  \n",k,output_mas[k]);
			for(int j=0;j<K_svet;j++)
			{
				for(int k=0;k<K_sost;k++)
					{
						printf("%0.3f   ",W[1][j][k]);
					
					}
				printf("\n");
			}
			char c;
		scanf("%c",&c);*/
/////////////////////////////////////////////////////////////////////////////
/////////// Проверяем правильность полученных выходов			
			if((R[k]->get_axon()-output_mas[k])==-1) /////если полученные результаты не верны, регулируем веса
			{
				for(int j=0;j<K_svet;j++)
				{
					W[1][j][k]+=a*(input_mas[j]-W[1][j][k]);
					/*if (count_iter>192) {printf("W[1][%d][%d]=%0.3f   ",j,k,W[1][j][k]);
								char c;
								scanf("%c",&c);}*/
				}
				k=k-1;						/////снова подаем данные этого же обучающего примера 
			}
			else
			if((R[k]->get_axon()-output_mas[k])==1) /////если полученные результаты не верны, регулируем веса
			{
				for(int j=0;j<K_svet;j++)
				{
					W[1][j][k]-=a*(input_mas[j]-W[1][j][k]);
					/*if (count_iter>192) {printf("W[1][%d][%d]=%0.3f   ",j,k,W[1][j][k]);
								char c;
								scanf("%c",&c);}*/
				}
				k=k-1;						/////снова подаем данные этого же обучающего примера 
			}
		}
	}
	W_writer(Wtek);
	/////////////////////////////////////////////////////////////////////////
	/*printf("The array of weighting coefficients \n");
			for(int j=0;j<K_svet;j++)
			{
				for(int k=0;k<K_sost;k++)
					{
						printf("%0.3f   ",W[1][j][k]);
					
					}
				printf("\n");
			}
			char c;
			scanf("%c",&c);*/
	
	fclose(Education_example);
}


///////////S_NEURON///////////////////////////////////////////
void S_neuron::activation()
{
	mem_axon=get_axon();
	set_axon(Data.Pk+Data.stas_Pk+Data.Tp+Data.Ck);
};

S_neuron::S_neuron(Enter_data D)
{
	set_axon(0);
	set_data(D);
	set_state(0);
};

void S_neuron::set_data(Enter_data D)
{
	Data = D;
	activation();
}
void S_neuron::activation(int k_svet){}
//////////////////////////////////////////////////////////////

/////////////////H_NEURON////////////////////////////////////
void H_neuron::activation(int k_svet)
{
	float X=0;
	for (int i=0;i<k_svet;i++)
	{
		X+=Dendrits[i];
	};
	X=0-X;
//	float a = -0.1;
	float e=2.71828;
	float result = 1/(1+pow(e,X));
	mem_axon=get_axon();
	set_axon(result);
};
H_neuron::H_neuron(float k_svet)
{
	k_d=k_svet;
	Dendrits= new float[k_d];
	set_state(0);
	set_axon(0);
};
/////////////////////////////////////////////////////////////

//////////////R_NEURON//////////////////////////////////////
R_neuron::R_neuron(float k_svet)
{
	k_d = k_svet;
	Dendrits =new float[k_d];
	set_state(0);
	set_axon(0);
}
void R_neuron::activation(int k_svet)
{
	float X=0;
	for (int i=0;i<k_svet;i++)
	{
		X+=Dendrits[i];
	};
	float T=0.31;
	/*wchar_t     szText2[250];
	swprintf(szText2,  L"X=%f",  X);
    MessageBox(NULL,  szText2,  L"ID",  MB_OK);*/
	set_axon(X);
	/*if (X<T)
		set_axon(0);
	else
		set_axon(1);*/
};
/////////////////////////////////////////////////////////////
void Neuro_crossRoad::Neuro_crossRoad_W0(char * f_name)
{
	for (int i=0;i<K_svet;i++)
	{
		for (int j=0;j<K_svet;j++)
		{
			W[0][i][j] = 0.001;
//			W[1][i][j] =(rand()%99);
//			W[1][i][j]=W[1][i][j]/10000;

		}
	}
	for (int i=0;i<K_svet;i++)
	{
		for (int j=0;j<K_sost;j++)
		{
			W[1][i][j] = 0.001;
//			W[1][i][j] =(rand()%99);
//			W[1][i][j]=W[1][i][j]/10000;

		}
	}
	W_writer(f_name);
}

void Neuro_crossRoad::W_writer(char * f_name)
{
	FILE * Ww;
	errno_t err;
	err = fopen_s(&Ww,f_name,"wt");
	for(int i=0;i<K_svet;i++)
	{
		for(int j=0; j<K_svet;j++)
		{
			fprintf(Ww,"%3.5f   ",W[0][i][j]);
		}
		fprintf(Ww,"\n");
	}

	fprintf(Ww,"\n");

	for(int i=0;i<K_svet;i++)
	{
		for(int j=0; j<K_sost;j++)
		{
			fprintf(Ww,"%3.5f   ",W[1][i][j]);
		}
		fprintf(Ww,"\n");
	}
	fclose(Ww);
}

void Neuro_crossRoad::W_reader(char * f_name)
{
	float l=0;
	FILE * Ww;
	errno_t err;
	err = fopen_s(&Ww, f_name, "rt");
	for(int i=0;i<K_svet;i++)
	{
		for(int j=0; j<K_svet;j++)
		{
			fscanf(Ww,"%f",&l);
			W[0][i][j]=l;
		}
	}

	for(int i=0;i<K_svet;i++)
	{
		for(int j=0; j<K_sost;j++)
		{
			fscanf(Ww,"%f",&l);
			W[1][i][j]=l;
		}
	}
	fclose(Ww);
}

Enter_data* Neuro_crossRoad::setData(Enter_data *Data, int k_svet) {
	for(int i=0; i<k_svet; i++) {
		D[i].Pk = Data[i].Pk;
		D[i].Ck = Data[i].Ck;
		D[i].Tp = Data[i].Tp;
		D[i].Pz = Data[i].Pz;
	}
	return D;
}