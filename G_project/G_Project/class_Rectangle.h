#pragma once
//----------------------------------------------------------------------------------------------------------------
#define DEFAULT_X -1
#define DEFAULT_Y 32000

class class_Rectangle
{
	private:
		long ID;
		int x;			// координаты
		int y;			// координаты
		int w;			// ширина прямоугольника
		int h;			// высота прямоугольника
		bool isOrient;	// можно ли его вращать (true - да, false - нет)
	
	public:
		// получение закрытых членов класса:
		int M_get_ID();
		int M_get_x();
		int M_get_y();
		int M_get_w();
		int M_get_h();
		bool M_get_isOrient();

		// установить все члены класса:
		void M_set_all(long iID, int ix, int iy, int iw, int ih, bool iisOrient);

		// установка закрытых членов класса:
		int M_set_x(int set_w);
		int M_set_y(int set_h);
		 		
		// метод который устанавливает координаты
		int M_set_coordinates(int constr_x, int constr_y);

		// метод заменяющий высоту и ширину
		int M_replacement_wh();

		// конструкторы:
		class_Rectangle(long ID, int constr_w, int constr_h, bool constr_isOrient);
		class_Rectangle();
		
		// деструктор:
		~class_Rectangle();
};
//----------------------------------------------------------------------------------------------------------------