#pragma once
//----------------------------------------------------------------------------------------------------------------
#define DEFAULT_X -1
#define DEFAULT_Y 32000

class class_Rectangle
{
	private:
		long ID;
		int x;			// ����������
		int y;			// ����������
		int w;			// ������ ��������������
		int h;			// ������ ��������������
		bool isOrient;	// ����� �� ��� ������� (true - ��, false - ���)
	
	public:
		// ��������� �������� ������ ������:
		int M_get_ID();
		int M_get_x();
		int M_get_y();
		int M_get_w();
		int M_get_h();
		bool M_get_isOrient();

		// ���������� ��� ����� ������:
		void M_set_all(long iID, int ix, int iy, int iw, int ih, bool iisOrient);

		// ��������� �������� ������ ������:
		int M_set_x(int set_w);
		int M_set_y(int set_h);
		 		
		// ����� ������� ������������� ����������
		int M_set_coordinates(int constr_x, int constr_y);

		// ����� ���������� ������ � ������
		int M_replacement_wh();

		// ������������:
		class_Rectangle(long ID, int constr_w, int constr_h, bool constr_isOrient);
		class_Rectangle();
		
		// ����������:
		~class_Rectangle();
};
//----------------------------------------------------------------------------------------------------------------