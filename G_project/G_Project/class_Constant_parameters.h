#pragma once
//----------------------------------------------------------------------------------------------------------------
class class_Constant_parameters
{
	private:
		int n;		// ���������� ��������������� (�� w, h, isOrient)
		int W;		// ����� ������, �� ������� �������������� �������� (������������ ������ ��������)
		int M;		// ����� ��������� (����� �������� ������������ ���������) - �������� �������� ���������
	
	public:
		// ��������� �������� ������ ������:
		int M_get_n();
		int M_get_W();
		int M_get_M();
		
		// ��������� �������� ������ ������:
		int M_set_n(int set_n);
		int M_set_W(int set_W);
		int M_set_M(int set_M);
		int M_set_all(int set_n, int set_W, int set_M);

		// ������������:
		class_Constant_parameters();
		class_Constant_parameters(int constr_n, int constr_W, int constr_M);					

		// ����������:
		~class_Constant_parameters();
};
//----------------------------------------------------------------------------------------------------------------