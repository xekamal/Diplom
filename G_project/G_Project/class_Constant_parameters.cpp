#include "stdafx.h"
#include "class_Constant_parameters.h"
												// получение закрытых членов класса:
int class_Constant_parameters::M_get_n()
{
	return n;
}
//----------------------------------------------------------------------------------------------------------------
int class_Constant_parameters::M_get_W()
{
	return W;
}
//----------------------------------------------------------------------------------------------------------------
int class_Constant_parameters::M_get_M()
{
	return M;
}
//----------------------------------------------------------------------------------------------------------------
												// установка закрытых членов класса:
int class_Constant_parameters::M_set_n(int set_n)
{
	n = set_n;
	if(n == set_n) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Constant_parameters::M_set_W(int set_W)
{
	W = set_W;
	if(W == set_W) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Constant_parameters::M_set_M(int set_M)
{
	M = set_M;
	if(M == set_M) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Constant_parameters::M_set_all(int set_n, int set_W, int set_M)
{
	n = set_n;
	W = set_W;
	M = set_M;
	if(n == set_n && W == set_W && M == set_M) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
												// конструкторы:
class_Constant_parameters::class_Constant_parameters()
{
	n = 0;
	W = 0;
	M = 0;
}
//----------------------------------------------------------------------------------------------------------------
class_Constant_parameters::class_Constant_parameters(int constr_n, int constr_W, int constr_M)					
{
	n = constr_n;
	W = constr_W;
	M = constr_M;
}
//----------------------------------------------------------------------------------------------------------------
												// деструктор:
class_Constant_parameters::~class_Constant_parameters()
{
	this->M_set_all(-1,-1,-1);
}
//----------------------------------------------------------------------------------------------------------------