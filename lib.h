// lib.h

#pragma once
//#define FILE_NAME "d:/Prog/neuro_lib/neuro_lib/Example.txt"

namespace lib {

///////////////////////////Класс входных данных/////////
	class Enter_data
	{
	public:
		
//		float Pk;	//плотность потока
//		float stas_Pk;
//		float Ck;	//оценка состояния
		float N;	//колличство машин за 1 сек, проезжающих 1 метр
		int Nmax; //
		int Nautos; //колличество машин,ждущих своей очереди на светофоре
		float V;  //скорость машин в потоке в км\ч
		int distance; // дистанция между машинами
		float Tp;	//время простоя
		float P;	//пропускная способность макс = 60 машин в минуту		
		float Pz;	//загруженность мин = 0,017
		float Pz_new;	//загруженность мин = 0,017
		void count_Pz_new();
		void recount_V(int);

		Enter_data();
	};
	////////////////////////////////////////////////////////
	///////////////////Класс нерона //////////////////////////////
	 class Neuron
	{
	private:
			int state;
			float axon;
			int ID;
	public:
		float * Dendrits;
		virtual void activation (int) = 0;
		float get_axon();
		void set_axon(float);
		void set_state(int);
		int get_state();
	};
	///////////////////////////////////////////////////////

	///////////////////////Класс сенсора /////////////////////////
	class S_neuron: public Neuron
	{
	private:
		Enter_data Data;
	public:
		float mem_axon;
		S_neuron(Enter_data);
		void activation();
		void activation(int);
		void set_data(Enter_data);

	};
	////////////////////////////////////////////////////////
	
	//////////////////Класс скрытого нейрона//////////////////
	class H_neuron: public Neuron
	{
	public:
		int k_d;
		float mem_axon;
		float E;	//для обучения
		float * Dendrits;
		void activation(int);
		void Set_Dendrits();
		H_neuron(float);
		
	};
	/////////////////////////////////////////////////////////

	///////////////////Класс результирующего нейрона/////////
	class R_neuron: public Neuron
	{
	public:
		int k_d;
		R_neuron(float);
		void activation(int);
		void Set_Dendrits();
	};
	////////////////////////////////////////////////////////

		/////////класс сети для перекрестка///////////////
	 class Neuro_crossRoad
	{
	private:
		int K_svet;
		int K_sost;
		int ID;
		int **States;
	public:
		char * Wtek;
		char * W0;
		float delta;
		float Tp_pred;
		float Pz_pred;
		Enter_data * Data_mas;
		S_neuron ** S;
		H_neuron ** H;
		R_neuron ** R;
		float *** W;

		Neuro_crossRoad(int,int,char *,char *,char *,Enter_data *);
		void Neuro_crossRoad_Create(void);
		void Neuro_crossRoad_W0(char * f_name);
		int * Neuro_crossRoad_Step(Enter_data *);
		void Neuro_crossRoad_Education_Hebb_S(Enter_data *);
		void Neuro_crossRoad_Education_Hebb_D(Enter_data *);
		void Neuro_crossRoad_Education_Kohonen(Enter_data *);
		void Neuro_crossRoad_Education_2(char*);
		void set_ID(int);
		void W_writer(char *);
		void W_reader(char *);
		void States_reader();
		void set_state(int *);
		void check_state();
		int get_Ksvet() { return K_svet; };
	};

	 class Traffic_Gen
	 {
	 public:
		 int k_svet;
		 Traffic_Gen(Enter_data *,int);
		 void gen_data(Enter_data *, Neuro_crossRoad, int *);
	 };
	////////////////////////////////////////////////
}
	

	



