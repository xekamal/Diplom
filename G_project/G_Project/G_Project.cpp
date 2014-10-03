// G_Project.cpp : main project file.

#include "stdafx.h"
#include "Form1.h"

#include "class_Constant_parameters.h"
#include "class_Engine_App.h"
#include "class_Rectangle.h"

using namespace G_Project;

[STAThreadAttribute]
int main(array<System::String ^> ^args)
{
	// Enabling Windows XP visual effects before any controls are created
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false); 

	// Create the main window and run it
	Application::Run(gcnew Form1());
	return 0;
}
