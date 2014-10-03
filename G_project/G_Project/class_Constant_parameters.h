#pragma once
//----------------------------------------------------------------------------------------------------------------
class class_Constant_parameters
{
	private:
		int n;		// количество прямоугольников (их w, h, isOrient)
		int W;		// длина полосы, на которой осуществляется упаковка (максимальная ширина упаковки)
		int M;		// число поколений (число итераций генетиеского алгоритма) - критерий останова алгоритма
	
	public:
		// получение закрытых членов класса:
		int M_get_n();
		int M_get_W();
		int M_get_M();
		
		// установка закрытых членов класса:
		int M_set_n(int set_n);
		int M_set_W(int set_W);
		int M_set_M(int set_M);
		int M_set_all(int set_n, int set_W, int set_M);

		// конструкторы:
		class_Constant_parameters();
		class_Constant_parameters(int constr_n, int constr_W, int constr_M);					

		// деструктор:
		~class_Constant_parameters();
};
//----------------------------------------------------------------------------------------------------------------