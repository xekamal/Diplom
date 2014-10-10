#pragma once

#include "lib.h"
#include "stdio.h"
#include "ResultForm.h"

#define file_W0 "d:/Study/W0.txt"
#define file_Wtek "d:/Study/Wtek.txt"
#define file_test "d:/Study/Test.txt"
#define K_svet 12					// Количество светофоров
#define K_sost 4					// Количество состояний


namespace CrossroadApp {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace lib;	
    using namespace std;


	int Nx = 6, Ny = 6;								// Количество клеток на поле
	int dx = 70, dy = 70;							// Ширина и высота клетки поля
	int chosenCrossroad = 0, chosenRoad = 0;		// Текущий выбранный тип перекрестка/дороги
	Neuro_crossRoad *** CrossroadArray;				// массив указателей на перекрестки
	int ** RoadArray;								// массив дорог
	int CrossroadID = 0;							//Количество установленных перекрестков
	FILE * Test;


	/// <summary>
	/// Сводка для Form1
	/// </summary>
	public ref class Form1 : public System::Windows::Forms::Form
	{
	public:
		Form1(void)
		{
			InitializeComponent();

			CrossroadArray = new Neuro_crossRoad ** [Nx];
			RoadArray = new int * [Nx];
			for (int i=0; i<Nx; i++) {
				CrossroadArray[i] = new Neuro_crossRoad * [Ny];
				RoadArray[i] = new int [Ny];
			}

			for ( int i = 0; i < Nx; ++i ) {
				for ( int j = 0; j < Ny; ++j ) {
					CrossroadArray[i][j] = NULL;
					RoadArray[i][j] = 0;
				}
			}

			errno_t err;
			err = fopen_s(&Test, file_test, "rt");

			printWindow();
			//
			//TODO: добавьте код конструктора
			//
		}

	protected:
		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		~Form1()
		{
			if (components)
			{
				delete components;
				fclose(Test);

				for ( int i = 0; i < Nx; i++ ) {
					for ( int j = 0; j < Ny; j++ ) {
						if (CrossroadArray[i][j] != NULL) delete CrossroadArray[i];
					}
				}
				for ( int i = 0; i < Nx; i++ ) {
					delete [] CrossroadArray[i];
					delete [] RoadArray[i];
				}
				delete [] CrossroadArray;
				delete [] RoadArray;
			}
		}


	private: System::Windows::Forms::TabControl^  tabControl1;
	private: System::Windows::Forms::TabPage^  tabPage1;
	private: System::Windows::Forms::TabPage^  tabPage2;

	private: System::Windows::Forms::PictureBox^  pictureBox1;
	private: System::Windows::Forms::Label^  LabelWorkField;

	private: System::Windows::Forms::Label^  LabelRoadTypes;
	private: System::Windows::Forms::PictureBox^  pictureBoxRTypes;

	private: System::Windows::Forms::Label^  LabelCrossRoad;
	private: System::Windows::Forms::PictureBox^  pictureBoxCrTypes;

	private: System::Windows::Forms::Button^  PrWindow;
	private: System::Windows::Forms::Button^  ButtonWebCreation;



	private:
		/// <summary>
		/// Требуется переменная конструктора.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		void InitializeComponent(void)
		{
			this->tabControl1 = (gcnew System::Windows::Forms::TabControl());
			this->tabPage1 = (gcnew System::Windows::Forms::TabPage());
			this->ButtonWebCreation = (gcnew System::Windows::Forms::Button());
			this->PrWindow = (gcnew System::Windows::Forms::Button());
			this->LabelRoadTypes = (gcnew System::Windows::Forms::Label());
			this->pictureBoxRTypes = (gcnew System::Windows::Forms::PictureBox());
			this->LabelCrossRoad = (gcnew System::Windows::Forms::Label());
			this->pictureBoxCrTypes = (gcnew System::Windows::Forms::PictureBox());
			this->LabelWorkField = (gcnew System::Windows::Forms::Label());
			this->pictureBox1 = (gcnew System::Windows::Forms::PictureBox());
			this->tabPage2 = (gcnew System::Windows::Forms::TabPage());
			this->tabControl1->SuspendLayout();
			this->tabPage1->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pictureBoxRTypes))->BeginInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pictureBoxCrTypes))->BeginInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pictureBox1))->BeginInit();
			this->SuspendLayout();
			// 
			// tabControl1
			// 
			this->tabControl1->Controls->Add(this->tabPage1);
			this->tabControl1->Controls->Add(this->tabPage2);
			this->tabControl1->Location = System::Drawing::Point(16, 13);
			this->tabControl1->Margin = System::Windows::Forms::Padding(4);
			this->tabControl1->Name = L"tabControl1";
			this->tabControl1->SelectedIndex = 0;
			this->tabControl1->Size = System::Drawing::Size(800, 525);
			this->tabControl1->TabIndex = 0;
			// 
			// tabPage1
			// 
			this->tabPage1->Controls->Add(this->ButtonWebCreation);
			this->tabPage1->Controls->Add(this->PrWindow);
			this->tabPage1->Controls->Add(this->LabelRoadTypes);
			this->tabPage1->Controls->Add(this->pictureBoxRTypes);
			this->tabPage1->Controls->Add(this->LabelCrossRoad);
			this->tabPage1->Controls->Add(this->pictureBoxCrTypes);
			this->tabPage1->Controls->Add(this->LabelWorkField);
			this->tabPage1->Controls->Add(this->pictureBox1);
			this->tabPage1->Location = System::Drawing::Point(4, 22);
			this->tabPage1->Margin = System::Windows::Forms::Padding(4);
			this->tabPage1->Name = L"tabPage1";
			this->tabPage1->Padding = System::Windows::Forms::Padding(4);
			this->tabPage1->Size = System::Drawing::Size(792, 499);
			this->tabPage1->TabIndex = 0;
			this->tabPage1->Text = L"tabPage1";
			this->tabPage1->UseVisualStyleBackColor = true;
			// 
			// ButtonWebCreation
			// 
			this->ButtonWebCreation->Location = System::Drawing::Point(157, 467);
			this->ButtonWebCreation->Name = L"ButtonWebCreation";
			this->ButtonWebCreation->Size = System::Drawing::Size(75, 23);
			this->ButtonWebCreation->TabIndex = 9;
			this->ButtonWebCreation->Text = L"CreateWeb";
			this->ButtonWebCreation->UseVisualStyleBackColor = true;
			this->ButtonWebCreation->Click += gcnew System::EventHandler(this, &Form1::Button_WebCreation_Click);
			// 
			// PrWindow
			// 
			this->PrWindow->Location = System::Drawing::Point(10, 467);
			this->PrWindow->Name = L"PrWindow";
			this->PrWindow->Size = System::Drawing::Size(75, 23);
			this->PrWindow->TabIndex = 8;
			this->PrWindow->Text = L"Print";
			this->PrWindow->UseVisualStyleBackColor = true;
			this->PrWindow->Click += gcnew System::EventHandler(this, &Form1::PrWindow_Click);
			// 
			// LabelRoadTypes
			// 
			this->LabelRoadTypes->AutoSize = true;
			this->LabelRoadTypes->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 15, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->LabelRoadTypes->Location = System::Drawing::Point(540, 150);
			this->LabelRoadTypes->Name = L"LabelRoadTypes";
			this->LabelRoadTypes->Size = System::Drawing::Size(118, 25);
			this->LabelRoadTypes->TabIndex = 7;
			this->LabelRoadTypes->Text = L"Road Types";
			// 
			// pictureBoxRTypes
			// 
			this->pictureBoxRTypes->Location = System::Drawing::Point(460, 180);
			this->pictureBoxRTypes->Name = L"pictureBoxRTypes";
			this->pictureBoxRTypes->Size = System::Drawing::Size(280, 140);
			this->pictureBoxRTypes->TabIndex = 6;
			this->pictureBoxRTypes->TabStop = false;
			this->pictureBoxRTypes->MouseClick += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::pictureBoxRTypes_MouseClick);
			// 
			// LabelCrossRoad
			// 
			this->LabelCrossRoad->AutoSize = true;
			this->LabelCrossRoad->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 15, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->LabelCrossRoad->Location = System::Drawing::Point(520, 10);
			this->LabelCrossRoad->Name = L"LabelCrossRoad";
			this->LabelCrossRoad->Size = System::Drawing::Size(163, 25);
			this->LabelCrossRoad->TabIndex = 5;
			this->LabelCrossRoad->Text = L"Crossroad Types";
			// 
			// pictureBoxCrTypes
			// 
			this->pictureBoxCrTypes->Location = System::Drawing::Point(460, 40);
			this->pictureBoxCrTypes->Name = L"pictureBoxCrTypes";
			this->pictureBoxCrTypes->Size = System::Drawing::Size(280, 70);
			this->pictureBoxCrTypes->TabIndex = 4;
			this->pictureBoxCrTypes->TabStop = false;
			this->pictureBoxCrTypes->MouseClick += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::pictureBoxCrTypes_MouseClick);
			// 
			// LabelWorkField
			// 
			this->LabelWorkField->AutoSize = true;
			this->LabelWorkField->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 15, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->LabelWorkField->Location = System::Drawing::Point(152, 9);
			this->LabelWorkField->Name = L"LabelWorkField";
			this->LabelWorkField->Size = System::Drawing::Size(106, 25);
			this->LabelWorkField->TabIndex = 3;
			this->LabelWorkField->Text = L"Work Field";
			// 
			// pictureBox1
			// 
			this->pictureBox1->Location = System::Drawing::Point(10, 40);
			this->pictureBox1->Margin = System::Windows::Forms::Padding(4);
			this->pictureBox1->Name = L"pictureBox1";
			this->pictureBox1->Size = System::Drawing::Size(420, 420);
			this->pictureBox1->TabIndex = 2;
			this->pictureBox1->TabStop = false;
			this->pictureBox1->MouseClick += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::pictureBox1_MouseClick);
			this->pictureBox1->MouseDoubleClick += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::pictureBox1_MouseDoubleClick);
			// 
			// tabPage2
			// 
			this->tabPage2->Location = System::Drawing::Point(4, 22);
			this->tabPage2->Margin = System::Windows::Forms::Padding(4);
			this->tabPage2->Name = L"tabPage2";
			this->tabPage2->Padding = System::Windows::Forms::Padding(4);
			this->tabPage2->Size = System::Drawing::Size(792, 499);
			this->tabPage2->TabIndex = 1;
			this->tabPage2->Text = L"tabPage2";
			this->tabPage2->UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(829, 568);
			this->Controls->Add(this->tabControl1);
			this->Margin = System::Windows::Forms::Padding(4);
			this->Name = L"Form1";
			this->Text = L"CrossroadApp";
			this->tabControl1->ResumeLayout(false);
			this->tabPage1->ResumeLayout(false);
			this->tabPage1->PerformLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pictureBoxRTypes))->EndInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pictureBoxCrTypes))->EndInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pictureBox1))->EndInit();
			this->ResumeLayout(false);

		}
#pragma endregion




private: System::Void pictureBox1_MouseClick(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e) {
	if ( e->Button == System::Windows::Forms::MouseButtons::Left) {
		Graphics^ g = pictureBox1->CreateGraphics();
		int x = e->X;
		int y = e->Y;

		int J = (x)/dx;
		int I = (y)/dy;

		if (CrossroadArray[I][J] == NULL && chosenCrossroad == 1 && RoadArray[I][J] == 0) {
			Enter_data * Data_CrossroadArray;
			Data_CrossroadArray = new Enter_data[K_svet];
	
			//считывание инф. из файла
			float l=0;
			if (!feof(Test) ) {
				for (int k=0;k<K_svet;k++) {
					fscanf(Test,"%f", &l);
					Data_CrossroadArray[k].Pk = l;
				}
			}

			CrossroadID = CrossroadID + 1;
			CrossroadArray[I][J] = new Neuro_crossRoad(K_svet, K_sost, file_W0, file_Wtek, Data_CrossroadArray, CrossroadID);
		}
		if (CrossroadArray[I][J] == NULL && chosenRoad != 0 && RoadArray[I][J] == 0) {
			RoadArray[I][J] = chosenRoad;
		}
		
		//Прорисовываем только что построенные перекресток/элемент дороги
		if (chosenCrossroad == 1) printCrossroad(J*dx, I*dy, g);
		if (chosenRoad != 0) printRoad(J*dx, I*dy, chosenRoad, g);
	}
}

private: System::Void pictureBoxCrTypes_MouseClick(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e) {
	if ( e->Button == System::Windows::Forms::MouseButtons::Left) {
		int x = e->X;
		int y = e->Y;

		int J = (x)/dx;
		int I = (y)/dy;
		if ((I == 0) && (J == 0)) chosenCrossroad = 1;
	}
	chosenRoad = 0;
}

private: System::Void pictureBoxRTypes_MouseClick(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e) {
	if ( e->Button == System::Windows::Forms::MouseButtons::Left) {
		int x = e->X;
		int y = e->Y;

		int J = (x)/dx;
		int I = (y)/dy;
		if (I == 0) chosenRoad = J+1;
			else chosenRoad = J+1+4;
	}
	chosenCrossroad = 0;
}

private: System::Void PrWindow_Click(System::Object^  sender, System::EventArgs^  e) {
	printWindow();
}

private: System::Void Button_WebCreation_Click(System::Object^  sender, System::EventArgs^  e) {
	for(int i=0; i<Nx; i++) {
		for(int j=0; j<Ny; j++) {
			if(CrossroadArray[i][j] != NULL ) {
				for(int c=0; c<K_svet; c++) {	//уляем значения Ck для формирования актуальной сети
					(*CrossroadArray[i][j]).D[c].Ck = 0;
				}
			}
		}
	}
	DataExchange();
	getState();	
}

private: System::Void pictureBox1_MouseDoubleClick(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e) {
		Graphics^ g = pictureBox1->CreateGraphics();
		int x = e->X;
		int y = e->Y;

		int J = (x)/dx;
		int I = (y)/dy;

		RoadArray[I][J] = 0;
		CrossroadArray[I][J] = NULL;

		printWindow();
}


void DataExchange() {
	float currentData = 0;		//для присвоения значений элементу CK

	for(int i=0; i<Nx; i++) {
		for(int j=0; j<Ny; j++) {
			if(CrossroadArray[i][j] != NULL ) {
				//для некрайних ячеек
				if ((i>0) && (i<Nx-1) && (j>0) && (j<Ny-1)) { 
					//изучаем клетку сверху от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
					getPath(i-1, j, currentData, 10);
					//изучаем клетку слева от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
					getPath(i, j-1, currentData, 11);
					//изучаем клетку справа от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
					getPath(i, j+1, currentData, 12);
					//изучаем клетку снизу от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
					getPath(i+1, j, currentData, 13);
					continue;
				}
				//для ячеек верхней строки
				if (i==0) { 
					if (j == 0) {
						//изучаем клетку справа от рассматриваемой
						currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
						getPath(i, j+1, currentData, 12);
						//изучаем клетку снизу от рассматриваемой
						currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
						getPath(i+1, j, currentData, 13);
						continue;
					} else {
						if (j == Ny-1) {
							//изучаем клетку слева от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
							getPath(i, j-1, currentData, 11);
							//изучаем клетку снизу от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
							getPath(i+1, j, currentData, 13);
							continue;
						} else {
							//изучаем клетку слева от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
							getPath(i, j-1, currentData, 11);
							//изучаем клетку справа от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
							getPath(i, j+1, currentData, 12);
							//изучаем клетку снизу от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
							getPath(i+1, j, currentData, 13);
							continue;
						}
					}
				}
				//для ячеек нижней строки
				if (i==Nx-1) { 
					if (j == 0) {
						//изучаем клетку сверху от рассматриваемой
						currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
						getPath(i-1, j, currentData, 10);
						//изучаем клетку справа от рассматриваемой
						currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
						getPath(i, j+1, currentData, 12);
						continue;
					} else {
						if (j == Ny-1) {
							//изучаем клетку сверху от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
							getPath(i-1, j, currentData, 10);
							//изучаем клетку слева от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
							getPath(i, j-1, currentData, 11);
							continue;
						} else {
							//изучаем клетку сверху от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
							getPath(i-1, j, currentData, 10);
							//изучаем клетку слева от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
							getPath(i, j-1, currentData, 11);
							//изучаем клетку справа от рассматриваемой
							currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
							getPath(i, j+1, currentData, 12);
							continue;
						}
					}
				}
				//для ячеек первого ряда(кроме угловых)
				if ((i>0) && (i<Nx-1) && (j == 0)) { 
					//изучаем клетку сверху от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
					getPath(i-1, j, currentData, 10);
					//изучаем клетку справа от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
					getPath(i, j+1, currentData, 12);
					//изучаем клетку снизу от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
					getPath(i+1, j, currentData, 13);
					continue;
				}
				//для ячеек крайнего правого ряда(кроме угловых)
				if ((i>0) && (i<Ny-1) && (j == Nx-1)) { 
					//изучаем клетку сверху от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
					getPath(i-1, j, currentData, 10);
					//изучаем клетку слева от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
					getPath(i, j-1, currentData, 11);
					//изучаем клетку снизу от рассматриваемой
					currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
					getPath(i+1, j, currentData, 13);
				}
			}
		}
	}
}

void getPath (int i, int j, float currentData, int direction) {
	switch (direction) {
		case 10: 
			while (RoadArray[i][j] == 2) {
				i = i-1;
			}
			if (CrossroadArray[i][j] != NULL) { 
				(*CrossroadArray[i][j]).D[1].Ck = currentData;
				(*CrossroadArray[i][j]).D[8].Ck = currentData;	
				(*CrossroadArray[i][j]).D[10].Ck = currentData;
				break;
			}
			switch (RoadArray[i][j]) {
				case 4: 
					getPath(i, j-1, currentData, 11);
					break;
				case 6:
					getPath(i, j+1, currentData, 12);
					break;
			}
			break;
		case 11:
			while (RoadArray[i][j] == 1) {
				j = j-1;
			}
			if (CrossroadArray[i][j] != NULL) { 
				(*CrossroadArray[i][j]).D[2].Ck = currentData;
				(*CrossroadArray[i][j]).D[5].Ck = currentData;	
				(*CrossroadArray[i][j]).D[11].Ck = currentData;
				break;
			}
			switch (RoadArray[i][j]) {
				case 5: 
					getPath(i-1, j, currentData, 10);
					break;
				case 6:
					getPath(i+1, j, currentData, 13);
					break;
			}
			break;
		case 12:
			while (RoadArray[i][j] == 1) {
				j = j+1;
			}
			if (CrossroadArray[i][j] != NULL) { 
				(*CrossroadArray[i][j]).D[3].Ck = currentData;
				(*CrossroadArray[i][j]).D[7].Ck = currentData;	
				(*CrossroadArray[i][j]).D[9].Ck = currentData;
				break;
			}
			switch (RoadArray[i][j]) {
				case 3: 
					getPath(i-1, j, currentData, 10);
					break;
				case 4:
					getPath(i+1, j, currentData, 13);
					break;
			}			
			break;
		case 13:
			while (RoadArray[i][j] == 2) {
				i = i+1;
			}
			if (CrossroadArray[i][j] != NULL) { 
				(*CrossroadArray[i][j]).D[0].Ck = currentData;
				(*CrossroadArray[i][j]).D[4].Ck = currentData;	
				(*CrossroadArray[i][j]).D[6].Ck = currentData;
				break;
			}
			switch (RoadArray[i][j]) {
				case 3: 
					getPath(i, j-1, currentData, 11);
					break;
				case 5:
					getPath(i, j+1, currentData, 12);
					break;
			}
	    	break;
		}
}

void getState() {
	ResultForm^ ResForm   =   gcnew ResultForm;        
    ResForm->Show();

	float* resultArr;
	resultArr = new float[K_sost];
	for(int i=0; i<K_sost; i++) {
		resultArr[i] = 0;
	}

	int count = 0;
	for(int i=0; i<Nx; i++) {
		for(int j=0; j<Ny; j++) {
			if(CrossroadArray[i][j] != NULL ) {
				resultArr = (*(CrossroadArray[i][j])).Neuro_crossRoad_Step((*(CrossroadArray[i][j])).D);

				float max = resultArr[0];
				int max_ind = 0;
				for(int a=1; a<K_sost; a++) {
					if(resultArr[a] > max) {
						max = resultArr[a];
						max_ind = a;
					}
				}
				for(int s=0; s<K_sost; s++) {
					if(s == max_ind) {
						resultArr[s] = 1;
					} else resultArr[s] = 0;
				}			

				ResForm->labelArr[count]->Text = "ID = " +  Convert::ToString((*(CrossroadArray[i][j])).getID())
									+ " Optimal state = " + Convert::ToString(max_ind + 1);
				count++;
				/*switch (count) {
				case 1:
					LabelResult1->Text = "Results: ID = " +  Convert::ToString((*(CrossroadArray[i][j])).getID())
									+ " Optimal state = " + Convert::ToString(max_ind + 1);
					break;
				case 2:
				LabelResult2->Text = "Results: ID = " +  Convert::ToString((*(CrossroadArray[i][j])).getID())
									+ " Optimal state = " + Convert::ToString(max_ind + 1);
					break;
				case 3:
				LabelResult3->Text = "Results: ID = " +  Convert::ToString((*(CrossroadArray[i][j])).getID())
									+ " Optimal state = " + Convert::ToString(max_ind + 1);
					break;
				case 4:
				LabelResult4->Text = "Results: ID = " +  Convert::ToString((*(CrossroadArray[i][j])).getID())
									+ " Optimal state = " + Convert::ToString(max_ind + 1);
					break;
				}*/
			}
		}
	}
}

void printCells(int X0, int Y0, int XCellsQuantity, int YCellsQuantity, Graphics^ g) {
	Pen^ blackPen = gcnew Pen(Color::Black); 
	for(int i=0; i<YCellsQuantity+1; i++) { //прорисовка горизонтальных линий
		g->DrawLine(blackPen, X0, Y0+i*dy, X0+XCellsQuantity*dx, Y0+i*dy);
	}
	for(int j=0; j<Ny+1; j++) { //прорисовка вертикальных линий
		g->DrawLine(blackPen, X0+j*dx, Y0, X0+j*dx, Y0+YCellsQuantity*dy);	
	}
}

void printCrossroad(int x, int y, Graphics^ g) {
	Pen^ blackPen = gcnew Pen(Color::Black);

	for(int i=0; i<3; i++) { //прорисовка горизонтальных линий 
		g->DrawLine(blackPen, x, (y+20+5*i), (x+20+5*i), (y+20+5*i));
		g->DrawLine(blackPen, (x+35+5*(3-i)), (y+20+5*i), (x+70), (y+20+5*i));
	}
	for(int i=0; i<3; i++) { //прорисовка горизонтальных линий 
		g->DrawLine(blackPen, x, (y+40+5*i), (x+20+5*(2-i)), (y+40+5*i));
		g->DrawLine(blackPen, (x+40+5*i), (y+40+5*i), (x+70), (y+40+5*i));
	}
	
	for(int j=0; j<3; j++) { //прорисовка вертикальных линий
		g->DrawLine(blackPen, (x+20+5*j), y, (x+20+5*j), (y+20+5*j));
		g->DrawLine(blackPen, (x+20+5*j), (y+40+5*(2-j)), (x+20+5*j), (y+70));
	}		
	for(int j=0; j<3; j++) { //прорисовка вертикальных линий
		g->DrawLine(blackPen, (x+40+5*j), y, (x+40+5*j), (y+20+5*(2-j)));
		g->DrawLine(blackPen, (x+40+5*j), (y+40+5*j), (x+40+5*j), (y+70));
	}
	//центральная полоса
	g->DrawLine(blackPen, x+35, y, x+35, y+70);
	g->DrawLine(blackPen, x, y+35, x+70, y+35);
}

void printRoad(int X0, int Y0, int roadType, Graphics^ g) {
	Pen^ blackPen = gcnew Pen(Color::Black);
	switch (roadType) {
		case 1:
			for(int i=0; i<7; i++) { //прорисовка горизонтальных линий
				g->DrawLine(blackPen, X0, (Y0+20+5*i), (X0+dx), (Y0+20+5*i));
			}
			break;
		case 2:
			for(int j=0; j<7; j++) { //прорисовка вертикальных линий
				g->DrawLine(blackPen, (X0+20+5*j), Y0, (X0+20+5*j), (Y0+dy));
			}	
			break;
		case 3:
			for(int i=0; i<7; i++) { //прорисовка горизонтальных линий 
				g->DrawLine(blackPen, X0, (Y0+20+5*i), (X0+20+5*i), (Y0+20+5*i));
			}
			for(int j=0; j<7; j++) { //прорисовка вертикальных линий
				g->DrawLine(blackPen, (X0+20+5*j), Y0, (X0+20+5*j), (Y0+20+5*j));
			}
			break;
		case 4:
			for(int i=0; i<7; i++) { //прорисовка горизонтальных линий 	
				g->DrawLine(blackPen, X0, (Y0+20+5*i), (X0+20+5*(6-i)), (Y0+20+5*i));
			}
			for(int j=0; j<7; j++) { //прорисовка вертикальных линий
				g->DrawLine(blackPen, (X0+20+5*j), (Y0+20+5*(6-j)), (X0+20+5*j), (Y0+dy));
			}
			break;
		case 5:
			for(int i=0; i<7; i++) { //прорисовка горизонтальных линий 
				g->DrawLine(blackPen, (X0+20+5*i), (Y0+20+5*(6-i)), (X0+dx), (Y0+20+5*(6-i)));
			}
			for(int j=0; j<7; j++) { //прорисовка вертикальных линий
				g->DrawLine(blackPen, (X0+20+5*j), Y0, (X0+20+5*j), (Y0+20+5*(6-j)));
			}
			break;
		case 6:
			for(int i=0; i<7; i++) { //прорисовка горизонтальных линий 
				g->DrawLine(blackPen, (X0+20+5*i), (Y0+20+5*i), (X0+dx), (Y0+20+5*i));
			}
			for(int j=0; j<7; j++) { //прорисовка вертикальных линий 
				g->DrawLine(blackPen, (X0+20+5*j), (Y0+20+5*j), (X0+20+5*j), (Y0+dy));
			}	
			break;
	}
}

void printWindow() {
	//прорисовка клеток рабочего окна
	Graphics^ g = pictureBox1->CreateGraphics();
	g->Clear(Color::White);
	printCells(0, 0, Nx, Ny, g);
		
	//прорисовка клеток окна макетов перекрестков
	Graphics^ gCrTypes = pictureBoxCrTypes->CreateGraphics();
	gCrTypes->Clear(Color::White);
	printCells(0, 0, 4, 1, gCrTypes);
	printCrossroad(0, 0, gCrTypes);

	//прорисовка клеток окна макетов дорог
	Graphics^ gRTypes = pictureBoxRTypes->CreateGraphics();
	gRTypes->Clear(Color::White);
	printCells(0, 0, 4, 2, gRTypes);

	// прорисовка макетов дорог
	for(int h=0; h<6; h++) {
		if (h<4) printRoad(dx*h, 0, h+1, gRTypes);
			else printRoad(dx*(h-4), dy, h+1, gRTypes);
	}

	// прорисовка существующих элементов дороги и перекрестков
	for (int h=0; h<Nx; h++) {
		for (int v=0; v<Ny; v++) { 
			if (CrossroadArray[h][v] != 0) {
				printCrossroad(v*dx, h*dy, g);
			}
			if (RoadArray[h][v] != 0) {
				printRoad(v*dx, h*dy, RoadArray[h][v], g);
			}
		}
	}
}


};
}

