#include "stdafx.h"
#include "class_Rectangle.h"

											// получение закрытых членов класса:
int class_Rectangle::M_get_x()
{
	return x;
}
//----------------------------------------------------------------------------------------------------------------
int class_Rectangle::M_get_y()
{
	return y;
}

int class_Rectangle::M_get_ID()
{
	return ID;
}
int class_Rectangle::M_get_w()
{
	return w;
}
//----------------------------------------------------------------------------------------------------------------
int class_Rectangle::M_get_h()
{
	return h;
}
//----------------------------------------------------------------------------------------------------------------
bool class_Rectangle::M_get_isOrient()
{
	return isOrient;
}

//----------------------------------------------------------------------------------------------------------------
void class_Rectangle::M_set_all(long iID, int ix, int iy, int iw, int ih, bool iisOrient)
{
	this->ID = iID;
	this->x = ix;
	this->y = iy;
	this->w = iw;
	this->h = ih;
	this->isOrient = iisOrient;
}
//----------------------------------------------------------------------------------------------------------------
												// установка закрытых членов класса:
int class_Rectangle::M_set_x(int set_x)
{
	x = set_x;
	if(x == set_x) return 0;
	else return 1;
}
//----------------------------------------------------------------------------------------------------------------
int class_Rectangle::M_set_y(int set_y)
{
	y = set_y;
	if(y == set_y) return 0;
	else return 1;
}

//----------------------------------------------------------------------------------------------------------------
												// метод который устанавливает координаты
int class_Rectangle::M_set_coordinates(int constr_x, int constr_y)
{
	x = constr_x;
	y = constr_y;
	if( constr_x && constr_y) return 0;
	else return 1;
}

												// метод заменяющий высоту и ширину
int class_Rectangle::M_replacement_wh()
{
	int temp;
	temp = w;
	w = h;
	h = temp;
	return 0;
}


//----------------------------------------------------------------------------------------------------------------
												// конструкторы:
class_Rectangle::class_Rectangle()
{
	x = DEFAULT_X;
	y = DEFAULT_Y;
	ID = -1;
}
//----------------------------------------------------------------------------------------------------------------
class_Rectangle::class_Rectangle(long id, int constr_w, int constr_h, bool constr_isOrient)
{
	x = DEFAULT_X;
	y = DEFAULT_Y;
	ID = id;
	w = constr_w;
	h = constr_h;
	isOrient = constr_isOrient;
}
//----------------------------------------------------------------------------------------------------------------
												// деструктор:
class_Rectangle::~class_Rectangle()
{
	x = DEFAULT_X;
	y = DEFAULT_Y;
	ID = -1;
	w = 0;
	h = 0;
	isOrient = 0;
}
//----------------------------------------------------------------------------------------------------------------