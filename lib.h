// lib.h

#pragma once
//#define FILE_NAME "d:/Prog/neuro_lib/neuro_lib/Example.txt"

namespace lib {

///////////////////////////����� ������� ������/////////
	class Enter_data
	{
	public:
		
//		float Pk;	//��������� ������
//		float stas_Pk;
//		float Ck;	//������ ���������
		float N;	//���������� ����� �� 1 ���, ����������� 1 ����
		int Nmax; //
		int Nautos; //����������� �����,������ ����� ������� �� ���������
		float V;  //�������� ����� � ������ � ��\�
		int distance; // ��������� ����� ��������
		float Tp;	//����� �������
		float P;	//���������� ����������� ���� = 60 ����� � ������		
		float Pz;	//������������� ��� = 0,017
		float Pz_new;	//������������� ��� = 0,017
		void count_Pz_new();
		void recount_V(int);

		Enter_data();
	};
	////////////////////////////////////////////////////////
	///////////////////����� ������ //////////////////////////////
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

	///////////////////////����� ������� /////////////////////////
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
	
	//////////////////����� �������� �������//////////////////
	class H_neuron: public Neuron
	{
	public:
		int k_d;
		float mem_axon;
		float E;	//��� ��������
		float * Dendrits;
		void activation(int);
		void Set_Dendrits();
		H_neuron(float);
		
	};
	/////////////////////////////////////////////////////////

	///////////////////����� ��������������� �������/////////
	class R_neuron: public Neuron
	{
	public:
		int k_d;
		R_neuron(float);
		void activation(int);
		void Set_Dendrits();
	};
	////////////////////////////////////////////////////////

		/////////����� ���� ��� �����������///////////////
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
	

	



