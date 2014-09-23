// Главный DLL-файл.

#include "lib.h"
#include <iostream>
#include <stdio.h>
#include <math.h>
//#define FILE_NAME "d:/Prog/neuro_lib/neuro_lib/Example.txt"

using namespace lib;
using namespace std;

Enter_data::Enter_data()
{
	recount_V(80);
	distance = 3;
	Nmax= V/(1+distance);
	N=V/(1+distance);
	Pz=N/Nmax;
	Tp=0;
	Nautos=0;
}
void Enter_data::recount_V(int v)
{
	V=(v*1000)/3600;
}
void Enter_data::count_Pz_new()
{
	
}
Traffic_Gen::Traffic_Gen(Enter_data *Data_mas, int K)
{
	k_svet = K;
//	Data_mas= new Enter_data[k_svet];
	printf("Enter data 0 :\n");
	for (int i=0;i<k_svet;i++)
	{
		Data_mas[i].recount_V((rand()%80));
		Data_mas[i].distance = (1+rand()%5);
		Data_mas[i].Pz_new = (Data_mas[i].V/(1+Data_mas[i].distance))/Data_mas[i].Nmax;
		Data_mas[i].Pz =Data_mas[i].Pz_new; 
		printf("Pz =%3.5f\n",Data_mas[i].Pz);
	}
	char c;
	scanf("%c",&c);
}
void Traffic_Gen::gen_data(Enter_data *Data_mas, Neuro_crossRoad CrossRoad, int * result_mass)
{	
	//Новая генерация
	printf("REEnter data :\n");
	for(int i=0;i<CrossRoad.get_Ksvet();i++)
	{
		if(CrossRoad.H[i]->get_state()==0)  //если светофор был выключен
		{
			Data_mas[i].Tp++;					//увеличиваем время простоя
			Data_mas[i].Pz_new=Data_mas[i].Pz + Data_mas[i].Pz_new; //увеличиваем загруженность на сгенерированный кусок
			Data_mas[i].Nautos+=Data_mas[i].N*60;
		}
		if(CrossRoad.H[i]->get_state()==1) // если светофор был включен
		{
			Data_mas[i].Tp=0;				//обнуляем врмя простоя
			if(Data_mas[i].Nautos>=Data_mas[i].N*60)
				Data_mas[i].Nautos-=Data_mas[i].N*60; // колличество машин под светофором уменьшилось
			else Data_mas[i].Nautos=0;
			Data_mas[i].Pz = (Data_mas[i].V/(1+Data_mas[i].distance))/Data_mas[i].Nmax; // пересчитываем новую загруженность

			
		}
		Data_mas[i].Pz = Data_mas[i].Pz_new; // после пересчета загруженностей выставляем текущую загруженность
		printf("Pz =%3.5f\n",Data_mas[i].Pz);

		//Новая генерация
		printf("New gen data :\n");
		Data_mas[i].V = (rand()%80);	//генерируем случайную скорость  машин
		Data_mas[i].distance = (rand()%5);  //генерируем случайную дистанцию
		Data_mas[i].Pz_new = (Data_mas[i].V/(1+Data_mas[i].distance))/Data_mas[i].Nmax; //считаем дополнительный кусок загруженности 
		
		printf("Pz_new =%3.5f\n",Data_mas[i].Pz_new);
	}
	char c;
	scanf("%c",&c);
}
void Neuro_crossRoad::check_state ()
{
	int f=0;
	for(int i=0;i<K_sost;i++)
	{
		if (R[i]->get_axon()!=0)
			f++;
		if(f>1)
		{
			for(int j=0;j<K_sost;j++)
			{
				if(i!=j)
					R[i]->set_axon(0);
			}
		}
	}
	if(f==0)
		R[0]->set_axon(1);
}
void Neuro_crossRoad::set_state(int * res_mas)
{
	int f=1;
	for (int j=0; j<K_sost;j++)
	{
		for(int i=0;i<K_sost;i++)
		{
			if(res_mas[i]!=States[j][i])
			{
				f=-1;
				j++;
				break;
			}
		}
		if(f==1)
		{
			for(int k=K_sost;k<K_svet+K_sost;k++)
			{
				H[k-K_sost]->set_state(States[j][k]);
				f=0;
		/*		printf("states: %d ",H[k-K_sost]->get_state());
				char ch;
				scanf("%c", &ch);
			*/
			}
		}
	}
}
void Neuron::set_axon(float a)
{
	axon =a;
}
float Neuron::get_axon()
{
	return axon;
}
int Neuron::get_state()
{
	return state;
}
void Neuron::set_state(int s)
{
	state = s;
}
void Neuro_crossRoad::set_ID(int id)
{
	ID=id;
}

Neuro_crossRoad::Neuro_crossRoad(int k_s, int k_sost, char * file_W0,char *file_Wtek,char *file_education, Enter_data *D)
{
	//Дефолтные входные данные 
	/*Data_mas[0].Pz = 0.3;
	Data_mas[1].Pz = 0.3;
	Data_mas[2].Pz = 0.3;	
	Data_mas[3].Pz = 0.5;
	Data_mas[4].Pz = 0.6;
	Data_mas[5].Pz = 0.5;
	Data_mas[6].Pz = 1.2;
	Data_mas[7].Pz = 1.5;
	Data_mas[8].Pz = 1.1;
	Data_mas[9].Pz = 0.3;
	Data_mas[10].Pz = 0.3;
	Data_mas[11].Pz = 0.2;
	*/
	Pz_pred=0;
//создание самой сети
	
	Wtek=file_Wtek;
	W0=file_W0;
	Tp_pred=0;
	K_svet = k_s;
	K_sost = k_sost;
	
	States = new int*[K_sost];
	for(int i=0;i<K_svet;i++)
	{
		States[i] = new int[K_svet+K_sost];
	}

	States_reader();
	
	S= new S_neuron*[K_svet];
	H= new H_neuron*[K_svet];
	R= new R_neuron*[K_sost];
	
	W = new float**[2];
	W[0] = new float*[K_svet];

	for (int i=0;i<K_svet;i++)
	{
		W[0][i] = new float[K_svet];
	}
/*	for (int i=0;i<K_svet;i++)
	{
		for (int j=0;j<K_svet;j++)
		{
			//W[0][i][j] = 0,001;
			W[0][i][j] =(rand()%99);
			W[0][i][j]=W[0][i][j]/1000;

		}
	}*/
	W[1] = new float*[K_svet];
	for (int i=0;i<K_svet;i++)
	{
		W[1][i] = new float[K_sost];
	}
	/*for (int i=0;i<K_svet;i++)
	{
		for (int j=0;j<K_sost;j++)
		{
			//W[1][i][j] = 0,001;
			W[1][i][j] =(rand()%99);
			W[1][i][j]=W[1][i][j]/100;
		}
	}*/
	
//	W_writer(file_W0,K_svet,K_sost);
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
		S[i] = new S_neuron(D[i]);
	}
	for (int i=0;i<K_svet;i++)
	{
		H[i] = new H_neuron(K_svet);
	}
	for (int i=0;i<K_sost;i++)
	{
		R[i] = new R_neuron(K_svet);
	}
//	W_writer(Wtek);
//	W_writer(file_W0);
//	Neuro_crossRoad_W0(file_W0);
	
	check_state ();
	int * def_mass = new int[K_sost];
	def_mass[0] = 1;
	def_mass[1] = 0;
	def_mass[2] = 0;
	def_mass[3] = 0;
	set_state(def_mass);
	
	W_reader(W0);
//	Neuro_crossRoad_Education_2(file_education);
//	W_writer(Wtek);
//	Neuro_crossRoad_Education_Hebb_S(D);
	delta =0;
}
int * Neuro_crossRoad::Neuro_crossRoad_Step( Enter_data * D)
{
	///////Передаем объекты входных данных сенсорам////////////////////
	for (int k=0;k<K_svet;k++)
	{
		S[k]->set_data(D[k]);
	}
//////////////////////////////////////////////////////////////////////////////////
	printf("axons S  \n");
	for(int j=0;j<K_svet;j++)
	{
		printf("%3.5f ",S[j]->get_axon());				
	}
	char c;
	scanf("%c",&c);
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
	printf("\n axons H  \n");
	for(int j=0;j<K_svet;j++)
	{
		printf("%3.5f ",H[j]->get_axon());
		printf("%d ",H[j]->get_state());
	}
//	char c;
	scanf("%c",&c);
////////////////////////////////////////////////////////////////////////////////////
//////самообучение сети без учителя
	Neuro_crossRoad_Education_Hebb_S(D);
//////////////////////////////////

	for(int j=0; j<K_sost; j++)
	{
		float summ=0;
		for (int i=0; i<K_svet;i++)
		{
			///на вход нейрона выходного слоя подается выходное значение нейрона скрытого слоя
			R[j]->Dendrits[i]=H[i]->get_axon()*W[1][i][j];
		  summ+=R[j]->Dendrits[i];
		}
		R[j]->activation(K_svet);////////////активация нейрона
	};
///////обратная связь	
	check_state ();
//////////////////////////////////////////////////////////////////////////////////
	printf("\n axons R  \n");
	for(int j=0;j<K_sost;j++)
	{
		printf("%3.5f ",R[j]->get_axon());				
	}
//	char c;
	scanf("%c",&c);
////////////////////////////////////////////////////////////////////////////////////

	///////////////////Полученные значения выходного слоя записываем в массив///////////////////
	int * mas;
	mas = new int[K_sost];
	for (int i=0;i<K_sost;i++)
		mas[i] = R[i]->get_axon();
	
////////////////////////////////////////////////////////////////////////////////////
	set_state(mas);
	return mas;
}
void Neuro_crossRoad::Neuro_crossRoad_Education_Hebb_S(Enter_data * D)
{
	float Tp_tek = 0;
	float Pz_tek = 0;
//	printf("\nTp_tek = %3.2f \n",Tp_tek);				

	float a = 0.001;   /////////////// коэфициент обучения
	int k=0;

		Tp_tek = 0;
		Pz_tek = 0;
		for (int i=0;i<K_svet;i++)
		{
			Tp_tek+=D[i].Tp;
			Pz_tek+=D[i].Pz_new;
		}	
		printf("\nPz_tek = %3.5f \n",Pz_tek);
		printf("\nPz_pred = %3.5f \n",Pz_pred);
		if ((Pz_pred -Pz_tek)<0)           ////если общая загруженность выросла
		{
			k++;
			for (int i=0;i<K_svet;i++)
			{
				for(int j=0;j<K_svet;j++)
				{
					W[0][i][j]+=a*S[i]->get_axon()*H[j]->get_axon(); /////регулируем веса
				}
			}
		}
/*		if ((Tp_tek-Tp_pred)>3)   
		{
			k++;
			for (int i=0;i<K_svet;i++)
			{
				for(int j=0;j<K_svet;j++)
				{
					W[0][i][j]+=a*S[i]->get_axon()*H[j]->get_axon();
				}
			}
//////////////////////////////////////////////////////////////////
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
/////////////////////////////////////////////////////////////////////
		Tp_pred=Tp_tek;
		Pz_pred=Pz_tek; 
		}
		else break; 
		*/
	Pz_pred=Pz_tek;
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
//	while (k<10)
//	{
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
	//	else break; 
//	}
	Tp_pred=Tp_tek;
}
void Neuro_crossRoad::Neuro_crossRoad_Education_2(char* file_education)
{
//	float T; // порог функции
//	W_reader(file_W0);
	float *input_mas = new float[K_svet];
	float *output_mas= new float[K_sost];
	int i=0;
	float a=0.01;
	errno_t err;
	FILE * Education_example;
	err = fopen_s(&Education_example,file_education,"rt");
	float l=0;
	while(!feof(Education_example) )
	{
		i=0;

		///////Считываем из файла строку с обучающим примером////////////////
		while (i< K_svet+K_sost)
		{
			if (i<K_svet)
			{
//				r=fscanf_s(Education_example,"%f", &l);
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
		printf("From file \n");
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
			printf("R[%d]->get_axon() =%f  ",k,R[k]->get_axon());
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
		scanf("%c",&c);
//////////////////////////////////////////////////////////////////////////////
/////////// Проверяем правильность полученных выходов			
			if((R[k]->get_axon()-output_mas[k])==-1) /////если полученные результаты не верны, регулируем веса
			{
				for(int j=0;j<K_svet;j++)
				{
					W[1][j][k]+=a*(input_mas[j]-W[1][j][k]);
//					printf("W[1][%d][%d]=%0.3f   ",j,k,W[1][j][k]);
				}
				k=k-1;						/////снова подаем данные этого же обучающего примера 
				W_writer(Wtek);
			}
			else
			if((R[k]->get_axon()-output_mas[k])==1) /////если полученные результаты не верны, регулируем веса
			{
				for(int j=0;j<K_svet;j++)
				{
					W[1][j][k]-=a*(input_mas[j]-W[1][j][k]);
//					printf("W[1][%d][%d]=%0.3f   ",j,k,W[1][j][k]);
				}
				k=k-1;						/////снова подаем данные этого же обучающего примера 
			}
		}

	
	}
	/////////////////////////////////////////////////////////////////////////
			for(int j=0;j<K_svet;j++)
			{
				for(int k=0;k<K_sost;k++)
					{
						printf("%0.3f   ",W[1][j][k]);
					
					}
				printf("\n");
			}
			char c;
			scanf("%c",&c);
	
	fclose(Education_example);
}


///////////S_NEURON///////////////////////////////////////////
void S_neuron::activation()
{
	float n =0.5;
	mem_axon=get_axon();
	set_axon(Data.Pz+pow(Data.Nautos,n)+pow(Data.Tp,n));
//	if(Data.Tp!=0)
//		set_axon(Data.Pz * pow(Data.Tp,n)+Data.Nautos+Data.Tp);
//	else
//		set_axon(Data.Pz);
};

S_neuron::S_neuron(Enter_data D)
{
	set_data(D);
	set_state(0);
	set_axon(0);

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
//	X=0-X;
	float a = 1.57;
	float e=2.71828;
//	float result = 1/(1+pow(e,X*a));
	float result = atan(X)/a;
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
	float T=0.3;
//	for (int i=0;i<k_sost;i++)
//	{
		
//	};
	if (X<T)
		set_axon(0);
	else
		set_axon(1);

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
	err = fopen_s(&Ww,f_name,"rt");
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
void Neuro_crossRoad::States_reader()
{
	float l=0;
	FILE * states;
	errno_t err;
	err = fopen_s(&states,"d:/Prog/lib/neuro_lib/states.txt","rt");
	for(int i=0;i<K_sost;i++)
	{
		for (int j=0;j<K_svet+K_sost;j++)
		{
			fscanf(states,"%f ",&l);
			this->States[i][j]=l;
		}
	}

/*	for(int i=0;i<K_sost;i++)
	{
		for (int j=0;j<K_svet+K_sost;j++)
		{
			printf("%d ", this->States[i][j]);
		}
		printf("\n");
	}
	char ch;
	scanf("%c", &ch);
	*/
	fclose(states);
}