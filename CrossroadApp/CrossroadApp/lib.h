// lib.h

#pragma once

namespace lib {

///////////////////////////����� ������� ������/////////
	class Enter_data
	{
	public:		
		float Pk;	//��������� ������
		float stas_Pk;
		float Tp;	//����� �������
		float Ck;	//������ ���������
		float Pz;
		float Speed;

//		void count_Pz(float);

		Enter_data();
	};
	////////////////////////////////////////////////////////
	///////////////////����� ������ //////////////////////////////
	 class Neuron
	{
	private:
			bool state;
			float axon;
			int ID;
	public:
		float * Dendrits;
		virtual void activation (int) = 0;
		float get_axon();
		void set_axon(float);
		void set_state(bool);
		bool get_state();
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
	public:
		char * Wtek;
		//float delta;
		float Tp_pred; 
		Enter_data * D;
		S_neuron ** S;
		H_neuron ** H;
		R_neuron ** R;
		float *** W;


		Neuro_crossRoad(int,int,char *,char *,Enter_data *, int);
		void Neuro_crossRoad_StartEducation(Enter_data *);
		void Neuro_crossRoad_W0(char * f_name);
		float * Neuro_crossRoad_Step(Enter_data *);
		void Neuro_crossRoad_Education_Hebb_S(Enter_data *);
		void Neuro_crossRoad_Education_Hebb_D(Enter_data *);
		void Neuro_crossRoad_Education_Kohonen(Enter_data *);
		void Neuro_crossRoad_Education_2();
		void W_writer(char *);
		void W_reader(char *);
		int getID();
		Enter_data* setData(Enter_data *, int);
	};

/*	 class Traffic_Gen
	 {
	 public:
		 Traffic_Gen(Enter_data *);
		 void gen_data(Enter_data *, int );
	 };*/
	////////////////////////////////////////////////
}
	

	




