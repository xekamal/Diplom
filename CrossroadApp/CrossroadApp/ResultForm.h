#pragma once

namespace CrossroadApp {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Сводка для ResultForm
	/// </summary>
	public ref class ResultForm : public System::Windows::Forms::Form
	{
	public:
		ResultForm(void)
		{
			InitializeComponent();
			//
			//TODO: добавьте код конструктора
			//
		}

	protected:
		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		~ResultForm()
		{
			if (components)
			{
				delete components;
			}
		}
	public: System::Windows::Forms::Label^  label1;
	cli::array<System::Windows::Forms::Label^>^ labelArr;
	private: System::Windows::Forms::Panel^  panel1;
	public: 

	protected: 

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
			// LabelArr
			labelArr = gcnew array<System::Windows::Forms::Label^>(50);
			for (int m=0; m<50; m++) {
				labelArr[m] = gcnew System::Windows::Forms::Label(); //создание объектов
				labelArr[m]->Size = System::Drawing::Size(200, 20);
				labelArr[m]->Location = System::Drawing::Point(15, 30+20*m);
			}

			this->label1 = (gcnew System::Windows::Forms::Label());
			this->panel1 = (gcnew System::Windows::Forms::Panel());
			this->panel1->SuspendLayout();
			this->SuspendLayout();
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(5, 5);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(15, 15);
			this->label1->TabIndex = 0;
			this->label1->Text = L"Results:";
			// 
			// panel1
			// 
			this->panel1->Controls->Add(this->label1);
			for (int m=0; m<50; m++) {
				this->panel1->Controls->Add(labelArr[m]);
			}
			this->panel1->Location = System::Drawing::Point(10, 10);
			this->panel1->Name = L"panel1";
			this->panel1->Size = System::Drawing::Size(265, 205);
			this->panel1->TabIndex = 1;
			// 
			// ResultForm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(5, 15);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(300, 300);
			this->Controls->Add(this->panel1);
			this->Name = L"ResultForm";
			this->Text = L"ResultForm";
			this->panel1->ResumeLayout(false);
			this->panel1->PerformLayout();
			this->ResumeLayout(false);

		}
#pragma endregion

	};
}
