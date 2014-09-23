#include "lib.h"
#include "iostream"
#define file_W0 "d:/Prog/lib/neuro_lib/W0.txt"
#define file_Wtek "d:/Prog/lib/neuro_lib/Wtek.txt"
//#define file_education "d:/Prog/lib/neuro_lib/Education_example.txt"
#define file_education "d:/Prog/lib/neuro_lib/example.txt"
using namespace lib;
void main()
{
	Enter_data *Data_mas;
	Data_mas=new Enter_data[12];

/*	for (int i=0;i<12;i++)
	{
		Data_mas[0].Pk = (rand()%99)/1000;
		Data_mas[0].Tp=(rand()%99)/1000;
		Data_mas[0].Ck = (rand()%99)/1000;
		Data_mas[0].stas_Pk=(rand()%99)/1000;
	}*/	
/*	Data_mas[0].Pz = 0.3;
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
	Neuro_crossRoad N(12,4, file_W0, file_Wtek, file_education,Data_mas );// Создали сеть, установили параметры по дефолту
	Traffic_Gen Traffic(Data_mas,N.get_Ksvet()); //Начальные входные данные
	for(int i=0;i<10;i++)
	{
		int *result_mas;
		result_mas = new int[4];
		result_mas=N.Neuro_crossRoad_Step(Data_mas); //Запустили сеть (обработка входных данных), получем массив состояний
	
		Traffic.gen_data(Data_mas,N,result_mas); //пересчет загружееностей, генерация новых входных данных
	}
	/*
	result_mas=N.Neuro_crossRoad_Step(Data_mas);

	printf("result \n");
	for(int j=0;j<4;j++)
	{
		printf("%d ",result_mas[j]);
	}
	char c;
	scanf("%c",&c);
	*/
}