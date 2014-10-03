 #pragma once

#include <stdlib.h> // rand
#include <ctime>	// time

#include "class_Constant_parameters.h"
#include "class_Engine_App.h"
#include "class_Rectangle.h"
#include "MethodsSelection.h"
//#include "Graphics.h"


namespace G_Project {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	
	/// <summary>
	/// Summary for Form1
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>

	public ref class Form1 : public System::Windows::Forms::Form
	{
	public:
		Form1(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			srand (unsigned(clock()));		// чтобы не повторялись "случайные" числа
			//
			// установим значения по умолчанию в ComboBox's
			Form1::decoder_input_field->SelectedIndex = 0;
			Form1::selection_input_field->SelectedIndex = 0;
			Form1::crossover_input_field->SelectedIndex = 0;
			//
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~Form1()
		{
			if (components)
			{
				delete components;
			}
		}
private: System::Windows::Forms::TabControl^  tabControl1;
protected: 
private: System::Windows::Forms::TabPage^  tabPage1;
private: System::Windows::Forms::TabPage^  tabPage2;
private: System::Windows::Forms::TabPage^  tabPage3;
private: System::Windows::Forms::TextBox^  textBox1;
private: System::Windows::Forms::Panel^  panel1;
private: System::Windows::Forms::Label^  label1;


private: System::Windows::Forms::Label^  label2;
private: System::Windows::Forms::Label^  label3;

private: System::Windows::Forms::Label^  label4;

private: System::Windows::Forms::Panel^  panel2;
private: System::Windows::Forms::Label^  label5;
private: System::Windows::Forms::Label^  label10;
private: System::Windows::Forms::Label^  label9;
private: System::Windows::Forms::Label^  label8;
private: System::Windows::Forms::Label^  label7;
private: System::Windows::Forms::Label^  label6;


private: System::Windows::Forms::ComboBox^  decoder_input_field;
private: System::Windows::Forms::ComboBox^  crossover_input_field;

private: System::Windows::Forms::ComboBox^  selection_input_field;
private: System::Windows::Forms::Button^  button_start;
private: System::Windows::Forms::NumericUpDown^  n_input_field;
private: System::Windows::Forms::NumericUpDown^  W_input_field;
private: System::Windows::Forms::NumericUpDown^  M_input_field;
private: System::Windows::Forms::NumericUpDown^  N_pop_input_field;
private: System::Windows::Forms::NumericUpDown^  K_input_field;

private: System::Windows::Forms::Label^  label_status;
	private: System::Windows::Forms::NumericUpDown^  tournament_size_input;


	private: System::Windows::Forms::Label^  label11;
	private: System::Windows::Forms::PictureBox^  graphic_panel;
	private: System::Windows::Forms::Button^  button_show;

	private: System::Windows::Forms::Button^  button_reset;
	private: System::Windows::Forms::Button^  btnOpenFirstPop;






	protected: 

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->tabControl1 = (gcnew System::Windows::Forms::TabControl());
			this->tabPage1 = (gcnew System::Windows::Forms::TabPage());
			this->btnOpenFirstPop = (gcnew System::Windows::Forms::Button());
			this->button_reset = (gcnew System::Windows::Forms::Button());
			this->label_status = (gcnew System::Windows::Forms::Label());
			this->button_start = (gcnew System::Windows::Forms::Button());
			this->panel2 = (gcnew System::Windows::Forms::Panel());
			this->label11 = (gcnew System::Windows::Forms::Label());
			this->tournament_size_input = (gcnew System::Windows::Forms::NumericUpDown());
			this->K_input_field = (gcnew System::Windows::Forms::NumericUpDown());
			this->N_pop_input_field = (gcnew System::Windows::Forms::NumericUpDown());
			this->crossover_input_field = (gcnew System::Windows::Forms::ComboBox());
			this->selection_input_field = (gcnew System::Windows::Forms::ComboBox());
			this->decoder_input_field = (gcnew System::Windows::Forms::ComboBox());
			this->label10 = (gcnew System::Windows::Forms::Label());
			this->label9 = (gcnew System::Windows::Forms::Label());
			this->label8 = (gcnew System::Windows::Forms::Label());
			this->label7 = (gcnew System::Windows::Forms::Label());
			this->label6 = (gcnew System::Windows::Forms::Label());
			this->label5 = (gcnew System::Windows::Forms::Label());
			this->panel1 = (gcnew System::Windows::Forms::Panel());
			this->M_input_field = (gcnew System::Windows::Forms::NumericUpDown());
			this->W_input_field = (gcnew System::Windows::Forms::NumericUpDown());
			this->n_input_field = (gcnew System::Windows::Forms::NumericUpDown());
			this->label4 = (gcnew System::Windows::Forms::Label());
			this->label3 = (gcnew System::Windows::Forms::Label());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->tabPage2 = (gcnew System::Windows::Forms::TabPage());
			this->button_show = (gcnew System::Windows::Forms::Button());
			this->graphic_panel = (gcnew System::Windows::Forms::PictureBox());
			this->tabPage3 = (gcnew System::Windows::Forms::TabPage());
			this->textBox1 = (gcnew System::Windows::Forms::TextBox());
			this->tabControl1->SuspendLayout();
			this->tabPage1->SuspendLayout();
			this->panel2->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->tournament_size_input))->BeginInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->K_input_field))->BeginInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->N_pop_input_field))->BeginInit();
			this->panel1->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->M_input_field))->BeginInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->W_input_field))->BeginInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->n_input_field))->BeginInit();
			this->tabPage2->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->graphic_panel))->BeginInit();
			this->tabPage3->SuspendLayout();
			this->SuspendLayout();
			// 
			// tabControl1
			// 
			this->tabControl1->Controls->Add(this->tabPage1);
			this->tabControl1->Controls->Add(this->tabPage2);
			this->tabControl1->Controls->Add(this->tabPage3);
			this->tabControl1->Location = System::Drawing::Point(12, 12);
			this->tabControl1->Name = L"tabControl1";
			this->tabControl1->SelectedIndex = 0;
			this->tabControl1->Size = System::Drawing::Size(984, 708);
			this->tabControl1->TabIndex = 1;
			// 
			// tabPage1
			// 
			this->tabPage1->Controls->Add(this->btnOpenFirstPop);
			this->tabPage1->Controls->Add(this->button_reset);
			this->tabPage1->Controls->Add(this->label_status);
			this->tabPage1->Controls->Add(this->button_start);
			this->tabPage1->Controls->Add(this->panel2);
			this->tabPage1->Controls->Add(this->panel1);
			this->tabPage1->Location = System::Drawing::Point(4, 22);
			this->tabPage1->Name = L"tabPage1";
			this->tabPage1->Padding = System::Windows::Forms::Padding(3);
			this->tabPage1->Size = System::Drawing::Size(976, 682);
			this->tabPage1->TabIndex = 0;
			this->tabPage1->Text = L"Settings";
			this->tabPage1->UseVisualStyleBackColor = true;
			// 
			// btnOpenFirstPop
			// 
			this->btnOpenFirstPop->Location = System::Drawing::Point(46, 271);
			this->btnOpenFirstPop->Name = L"btnOpenFirstPop";
			this->btnOpenFirstPop->Size = System::Drawing::Size(158, 23);
			this->btnOpenFirstPop->TabIndex = 5;
			this->btnOpenFirstPop->Text = L"Open the first population...";
			this->btnOpenFirstPop->UseVisualStyleBackColor = true;
			this->btnOpenFirstPop->Click += gcnew System::EventHandler(this, &Form1::btnOpenFirstPop_Click);
			// 
			// button_reset
			// 
			this->button_reset->Enabled = false;
			this->button_reset->Location = System::Drawing::Point(46, 230);
			this->button_reset->Name = L"button_reset";
			this->button_reset->Size = System::Drawing::Size(138, 23);
			this->button_reset->TabIndex = 4;
			this->button_reset->Text = L"Reset algorithm";
			this->button_reset->UseVisualStyleBackColor = true;
			this->button_reset->Click += gcnew System::EventHandler(this, &Form1::button_reset_Click);
			// 
			// label_status
			// 
			this->label_status->AutoSize = true;
			this->label_status->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 10));
			this->label_status->Location = System::Drawing::Point(43, 172);
			this->label_status->Name = L"label_status";
			this->label_status->Size = System::Drawing::Size(74, 17);
			this->label_status->TabIndex = 3;
			this->label_status->Text = L"status: n/a";
			// 
			// button_start
			// 
			this->button_start->Location = System::Drawing::Point(46, 195);
			this->button_start->Name = L"button_start";
			this->button_start->Size = System::Drawing::Size(138, 23);
			this->button_start->TabIndex = 2;
			this->button_start->Text = L"Start algorithm";
			this->button_start->UseVisualStyleBackColor = true;
			this->button_start->Click += gcnew System::EventHandler(this, &Form1::button_start_Click);
			// 
			// panel2
			// 
			this->panel2->BackColor = System::Drawing::Color::WhiteSmoke;
			this->panel2->Controls->Add(this->label11);
			this->panel2->Controls->Add(this->tournament_size_input);
			this->panel2->Controls->Add(this->K_input_field);
			this->panel2->Controls->Add(this->N_pop_input_field);
			this->panel2->Controls->Add(this->crossover_input_field);
			this->panel2->Controls->Add(this->selection_input_field);
			this->panel2->Controls->Add(this->decoder_input_field);
			this->panel2->Controls->Add(this->label10);
			this->panel2->Controls->Add(this->label9);
			this->panel2->Controls->Add(this->label8);
			this->panel2->Controls->Add(this->label7);
			this->panel2->Controls->Add(this->label6);
			this->panel2->Controls->Add(this->label5);
			this->panel2->Location = System::Drawing::Point(317, 19);
			this->panel2->Name = L"panel2";
			this->panel2->Size = System::Drawing::Size(384, 343);
			this->panel2->TabIndex = 1;
			// 
			// label11
			// 
			this->label11->AutoSize = true;
			this->label11->Location = System::Drawing::Point(275, 109);
			this->label11->Name = L"label11";
			this->label11->Size = System::Drawing::Size(87, 13);
			this->label11->TabIndex = 14;
			this->label11->Text = L"Tournament Size";
			this->label11->Visible = false;
			// 
			// tournament_size_input
			// 
			this->tournament_size_input->Location = System::Drawing::Point(278, 125);
			this->tournament_size_input->Maximum = System::Decimal(gcnew cli::array< System::Int32 >(4) {5, 0, 0, 0});
			this->tournament_size_input->Minimum = System::Decimal(gcnew cli::array< System::Int32 >(4) {2, 0, 0, 0});
			this->tournament_size_input->Name = L"tournament_size_input";
			this->tournament_size_input->Size = System::Drawing::Size(74, 20);
			this->tournament_size_input->TabIndex = 13;
			this->tournament_size_input->Value = System::Decimal(gcnew cli::array< System::Int32 >(4) {2, 0, 0, 0});
			this->tournament_size_input->Visible = false;
			// 
			// K_input_field
			// 
			this->K_input_field->Increment = System::Decimal(gcnew cli::array< System::Int32 >(4) {10, 0, 0, 0});
			this->K_input_field->Location = System::Drawing::Point(179, 196);
			this->K_input_field->Minimum = System::Decimal(gcnew cli::array< System::Int32 >(4) {10, 0, 0, 0});
			this->K_input_field->Name = L"K_input_field";
			this->K_input_field->Size = System::Drawing::Size(80, 20);
			this->K_input_field->TabIndex = 12;
			this->K_input_field->Value = System::Decimal(gcnew cli::array< System::Int32 >(4) {50, 0, 0, 0});
			// 
			// N_pop_input_field
			// 
			this->N_pop_input_field->Location = System::Drawing::Point(183, 47);
			this->N_pop_input_field->Maximum = System::Decimal(gcnew cli::array< System::Int32 >(4) {20, 0, 0, 0});
			this->N_pop_input_field->Minimum = System::Decimal(gcnew cli::array< System::Int32 >(4) {10, 0, 0, 0});
			this->N_pop_input_field->Name = L"N_pop_input_field";
			this->N_pop_input_field->Size = System::Drawing::Size(76, 20);
			this->N_pop_input_field->TabIndex = 11;
			this->N_pop_input_field->Value = System::Decimal(gcnew cli::array< System::Int32 >(4) {10, 0, 0, 0});
			// 
			// crossover_input_field
			// 
			this->crossover_input_field->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->crossover_input_field->FormattingEnabled = true;
			this->crossover_input_field->Items->AddRange(gcnew cli::array< System::Object^  >(2) {L"One-point", L"Two-point"});
			this->crossover_input_field->Location = System::Drawing::Point(138, 159);
			this->crossover_input_field->Name = L"crossover_input_field";
			this->crossover_input_field->Size = System::Drawing::Size(121, 21);
			this->crossover_input_field->TabIndex = 10;
			// 
			// selection_input_field
			// 
			this->selection_input_field->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->selection_input_field->FormattingEnabled = true;
			this->selection_input_field->Items->AddRange(gcnew cli::array< System::Object^  >(3) {L"Proportional", L"Rank", L"Tournament"});
			this->selection_input_field->Location = System::Drawing::Point(138, 124);
			this->selection_input_field->Name = L"selection_input_field";
			this->selection_input_field->Size = System::Drawing::Size(121, 21);
			this->selection_input_field->TabIndex = 9;
			this->selection_input_field->SelectedIndexChanged += gcnew System::EventHandler(this, &Form1::selection_input_field_SelectedIndexChanged);
			// 
			// decoder_input_field
			// 
			this->decoder_input_field->DisplayMember = L"0";
			this->decoder_input_field->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->decoder_input_field->FormattingEnabled = true;
			this->decoder_input_field->Items->AddRange(gcnew cli::array< System::Object^  >(2) {L"Bottom-Left", L"Improved Bottom-Left"});
			this->decoder_input_field->Location = System::Drawing::Point(138, 83);
			this->decoder_input_field->Name = L"decoder_input_field";
			this->decoder_input_field->Size = System::Drawing::Size(121, 21);
			this->decoder_input_field->TabIndex = 8;
			// 
			// label10
			// 
			this->label10->Location = System::Drawing::Point(23, 196);
			this->label10->Name = L"label10";
			this->label10->Size = System::Drawing::Size(143, 36);
			this->label10->TabIndex = 5;
			this->label10->Text = L"K(gen) - rate of formation of a new generation (in %)";
			// 
			// label9
			// 
			this->label9->AutoSize = true;
			this->label9->Location = System::Drawing::Point(23, 159);
			this->label9->Name = L"label9";
			this->label9->Size = System::Drawing::Size(77, 13);
			this->label9->TabIndex = 4;
			this->label9->Text = L"Crossover type";
			// 
			// label8
			// 
			this->label8->AutoSize = true;
			this->label8->Location = System::Drawing::Point(23, 124);
			this->label8->Name = L"label8";
			this->label8->Size = System::Drawing::Size(74, 13);
			this->label8->TabIndex = 3;
			this->label8->Text = L"Selection type";
			// 
			// label7
			// 
			this->label7->AutoSize = true;
			this->label7->Location = System::Drawing::Point(23, 90);
			this->label7->Name = L"label7";
			this->label7->Size = System::Drawing::Size(71, 13);
			this->label7->TabIndex = 2;
			this->label7->Text = L"Decoder type";
			// 
			// label6
			// 
			this->label6->AutoSize = true;
			this->label6->Location = System::Drawing::Point(23, 54);
			this->label6->Name = L"label6";
			this->label6->Size = System::Drawing::Size(100, 13);
			this->label6->TabIndex = 1;
			this->label6->Text = L"N- population count";
			// 
			// label5
			// 
			this->label5->AutoSize = true;
			this->label5->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 10, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->label5->Location = System::Drawing::Point(20, 17);
			this->label5->Name = L"label5";
			this->label5->Size = System::Drawing::Size(146, 17);
			this->label5->TabIndex = 0;
			this->label5->Text = L"Variable input data";
			// 
			// panel1
			// 
			this->panel1->BackColor = System::Drawing::Color::WhiteSmoke;
			this->panel1->Controls->Add(this->M_input_field);
			this->panel1->Controls->Add(this->W_input_field);
			this->panel1->Controls->Add(this->n_input_field);
			this->panel1->Controls->Add(this->label4);
			this->panel1->Controls->Add(this->label3);
			this->panel1->Controls->Add(this->label2);
			this->panel1->Controls->Add(this->label1);
			this->panel1->Location = System::Drawing::Point(21, 15);
			this->panel1->Name = L"panel1";
			this->panel1->Size = System::Drawing::Size(254, 141);
			this->panel1->TabIndex = 0;
			// 
			// M_input_field
			// 
			this->M_input_field->Location = System::Drawing::Point(140, 106);
			this->M_input_field->Maximum = System::Decimal(gcnew cli::array< System::Int32 >(4) {50, 0, 0, 0});
			this->M_input_field->Minimum = System::Decimal(gcnew cli::array< System::Int32 >(4) {3, 0, 0, 0});
			this->M_input_field->Name = L"M_input_field";
			this->M_input_field->Size = System::Drawing::Size(84, 20);
			this->M_input_field->TabIndex = 8;
			this->M_input_field->Value = System::Decimal(gcnew cli::array< System::Int32 >(4) {3, 0, 0, 0});
			// 
			// W_input_field
			// 
			this->W_input_field->Location = System::Drawing::Point(140, 70);
			this->W_input_field->Maximum = System::Decimal(gcnew cli::array< System::Int32 >(4) {10000, 0, 0, 0});
			this->W_input_field->Minimum = System::Decimal(gcnew cli::array< System::Int32 >(4) {10, 0, 0, 0});
			this->W_input_field->Name = L"W_input_field";
			this->W_input_field->Size = System::Drawing::Size(84, 20);
			this->W_input_field->TabIndex = 7;
			this->W_input_field->Value = System::Decimal(gcnew cli::array< System::Int32 >(4) {300, 0, 0, 0});
			// 
			// n_input_field
			// 
			this->n_input_field->Location = System::Drawing::Point(140, 30);
			this->n_input_field->Maximum = System::Decimal(gcnew cli::array< System::Int32 >(4) {200, 0, 0, 0});
			this->n_input_field->Minimum = System::Decimal(gcnew cli::array< System::Int32 >(4) {2, 0, 0, 0});
			this->n_input_field->Name = L"n_input_field";
			this->n_input_field->Size = System::Drawing::Size(84, 20);
			this->n_input_field->TabIndex = 3;
			this->n_input_field->Value = System::Decimal(gcnew cli::array< System::Int32 >(4) {10, 0, 0, 0});
			// 
			// label4
			// 
			this->label4->AutoSize = true;
			this->label4->Location = System::Drawing::Point(13, 106);
			this->label4->Name = L"label4";
			this->label4->Size = System::Drawing::Size(127, 13);
			this->label4->TabIndex = 5;
			this->label4->Text = L"M - max generation count";
			// 
			// label3
			// 
			this->label3->Location = System::Drawing::Point(13, 60);
			this->label3->Name = L"label3";
			this->label3->Size = System::Drawing::Size(107, 38);
			this->label3->TabIndex = 3;
			this->label3->Text = L"W - max width of the string to pack";
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Location = System::Drawing::Point(13, 36);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(123, 13);
			this->label2->TabIndex = 2;
			this->label2->Text = L"n - Number of rectangles";
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 10, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->label1->Location = System::Drawing::Point(13, 10);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(150, 17);
			this->label1->TabIndex = 1;
			this->label1->Text = L"Constant input data";
			// 
			// tabPage2
			// 
			this->tabPage2->Controls->Add(this->button_show);
			this->tabPage2->Controls->Add(this->graphic_panel);
			this->tabPage2->Location = System::Drawing::Point(4, 22);
			this->tabPage2->Name = L"tabPage2";
			this->tabPage2->Padding = System::Windows::Forms::Padding(3);
			this->tabPage2->Size = System::Drawing::Size(976, 682);
			this->tabPage2->TabIndex = 1;
			this->tabPage2->Text = L"Graphic";
			this->tabPage2->UseVisualStyleBackColor = true;
			// 
			// button_show
			// 
			this->button_show->Enabled = false;
			this->button_show->Location = System::Drawing::Point(15, 643);
			this->button_show->Name = L"button_show";
			this->button_show->Size = System::Drawing::Size(141, 23);
			this->button_show->TabIndex = 1;
			this->button_show->Text = L"Show packing";
			this->button_show->UseVisualStyleBackColor = true;
			this->button_show->Click += gcnew System::EventHandler(this, &Form1::btn_show_Click);
			// 
			// graphic_panel
			// 
			this->graphic_panel->BackColor = System::Drawing::Color::DarkGray;
			this->graphic_panel->Location = System::Drawing::Point(6, 6);
			this->graphic_panel->Name = L"graphic_panel";
			this->graphic_panel->Size = System::Drawing::Size(964, 606);
			this->graphic_panel->TabIndex = 0;
			this->graphic_panel->TabStop = false;
			// 
			// tabPage3
			// 
			this->tabPage3->Controls->Add(this->textBox1);
			this->tabPage3->Location = System::Drawing::Point(4, 22);
			this->tabPage3->Name = L"tabPage3";
			this->tabPage3->Padding = System::Windows::Forms::Padding(3);
			this->tabPage3->Size = System::Drawing::Size(976, 682);
			this->tabPage3->TabIndex = 2;
			this->tabPage3->Text = L"Debug log";
			this->tabPage3->UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this->textBox1->Location = System::Drawing::Point(21, 6);
			this->textBox1->Multiline = true;
			this->textBox1->Name = L"textBox1";
			this->textBox1->ScrollBars = System::Windows::Forms::ScrollBars::Both;
			this->textBox1->Size = System::Drawing::Size(939, 656);
			this->textBox1->TabIndex = 1;
			this->textBox1->Text = L"Debug log";
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->AutoScroll = true;
			this->AutoValidate = System::Windows::Forms::AutoValidate::EnablePreventFocusChange;
			this->ClientSize = System::Drawing::Size(1008, 732);
			this->Controls->Add(this->tabControl1);
			this->FormBorderStyle = System::Windows::Forms::FormBorderStyle::FixedSingle;
			this->Name = L"Form1";
			this->Text = L"Genetic algorithm";
			this->tabControl1->ResumeLayout(false);
			this->tabPage1->ResumeLayout(false);
			this->tabPage1->PerformLayout();
			this->panel2->ResumeLayout(false);
			this->panel2->PerformLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->tournament_size_input))->EndInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->K_input_field))->EndInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->N_pop_input_field))->EndInit();
			this->panel1->ResumeLayout(false);
			this->panel1->PerformLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->M_input_field))->EndInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->W_input_field))->EndInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->n_input_field))->EndInit();
			this->tabPage2->ResumeLayout(false);
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->graphic_panel))->EndInit();
			this->tabPage3->ResumeLayout(false);
			this->tabPage3->PerformLayout();
			this->ResumeLayout(false);

		}
#pragma endregion
private: System::Void button_start_Click(System::Object^  sender, System::EventArgs^  e) 
		 {
			// задействована кнопка старта, загружаем параметры в наш класс "class_Engine_App"
 			button_start->Enabled = false;
			button_reset->Enabled = true;
			button_show->Enabled = false;
			int N_pop(0), Dec(0), Sel(0), Cros(0), Koeffic(0), Joust(0);
			N_pop = (int)(Form1::N_pop_input_field->Value);
			Dec = (int)(Form1::decoder_input_field->SelectedIndex);
			Sel = (int)(Form1::selection_input_field->SelectedIndex);
			Cros = (int)(Form1::crossover_input_field->SelectedIndex);
			Koeffic = (int)(Form1::K_input_field->Value);

			// теперь загрузим оставшиеся константные параметры: 
			int n(0), W(0), M(0);
			n = (int)(Form1::n_input_field->Value);
			W = (int)(Form1::W_input_field->Value);
			M = (int)(Form1::M_input_field->Value);

			if (tournament_size_input->Visible)
			{
				Joust = (int)tournament_size_input->Value;
			}

			//getting an algorithm instance
			class_Engine_App* inst =class_Engine_App::getInstance();
			inst->M_main_func(N_pop, Dec, Sel, Cros, Koeffic, n, W, M, Joust);


			button_show->Enabled = true;



			//			Drow_Rectangles(graphic_panel);
			
			//Drow_Rectangles(graphic_panel,C_Start_Engine_App);	


		 }	 // Void button_start_Click
private: System::Void selection_input_field_SelectedIndexChanged(System::Object^  sender, System::EventArgs^  e) {
			 if (selection_input_field->SelectedItem->Equals("Tournament"))
			 {
				 tournament_size_input->Visible = true;
				 label11->Visible = true;
			 }
			 else {
				 tournament_size_input->Visible = false;
				 label11->Visible = false;
			 } 
		 }
 public: System::Void Drow_Rectangles(PictureBox^ graphic_panel_P)
			{	

				class_Engine_App* inst = class_Engine_App::getInstance();
				int i = inst->M_getFinalGenerationNumber();
				int j = inst->M_getBestPriorityIdx();

				//getting packing height
				int maxPackingHeight = inst->Array[i][j][0]->M_get_y()+inst->Array[i][j][0]->M_get_h();
				for (int rectIdx = 1; rectIdx < inst->C_Constant_parameters.M_get_n(); rectIdx++)
				{
					if 	(inst->Array[i][j][rectIdx]->M_get_y()+
							inst->Array[i][j][rectIdx]->M_get_h() > maxPackingHeight)
					{
						maxPackingHeight = inst->Array[i][j][rectIdx]->M_get_y()+
							inst->Array[i][j][rectIdx]->M_get_h();
					}
				}
				
				Graphics^ Rect = graphic_panel_P->CreateGraphics();
				System::Drawing::Font^ drawFont = gcnew System::Drawing::Font( "Arial",12 );
				SolidBrush^ drawBrush = gcnew SolidBrush( Color::Black );
				
				for(int k=0; k < inst->C_Constant_parameters.M_get_n();k++)
				{			
					Rectangle drawRect = Rectangle(inst->Array[i][j][k]->M_get_x()+10,
						graphic_panel->Height-10-(inst->Array[i][j][k]->M_get_h() + inst->Array[i][j][k]->M_get_y()),
						inst->Array[i][j][k]->M_get_w(),inst->Array[i][j][k]->M_get_h());
					Rect->DrawRectangle(Pens::Black,drawRect);
					Rect->DrawString(inst->Array[i][j][k]->M_get_ID().ToString(), drawFont, drawBrush, drawRect );
				}	
				//Rect->DrawString("Fin!", drawFont, drawBrush, Rectangle (10, graphic_panel->Height-10 - maxPackingHeight, 70, 70));
				Graphics^ Line =  graphic_panel_P->CreateGraphics();			
				Line->DrawLine(Pens::Blue, 10, graphic_panel->Height-10 ,10, 10);
				Line->DrawLine(Pens::Red, 10 + inst->C_Constant_parameters.M_get_W(), 10 , 10 + inst->C_Constant_parameters.M_get_W(),
					graphic_panel->Height-10);
				Line->DrawLine(Pens::Blue, 10, graphic_panel->Height-10 ,graphic_panel->Width-10,graphic_panel->Height-10);
			}
public: System::Void Drow_Rectangles_1(PictureBox^ graphic_panel_P)
		{
				class_Engine_App* inst = class_Engine_App::getInstance();
				int i = inst->M_getFinalGenerationNumber();
				int j = inst->M_getBestPriorityIdx();

				//getting packing height
				int maxPackingHeight = inst->Array[i][j][0]->M_get_y()+inst->Array[i][j][0]->M_get_h();
				graphic_panel_P->Height = (maxPackingHeight+50)*10;
				for (int rectIdx = 1; rectIdx < inst->C_Constant_parameters.M_get_n(); rectIdx++)
				{
					if 	(inst->Array[i][j][rectIdx]->M_get_y()+
							inst->Array[i][j][rectIdx]->M_get_h() > maxPackingHeight)
					{
						maxPackingHeight = inst->Array[i][j][rectIdx]->M_get_y()+
							inst->Array[i][j][rectIdx]->M_get_h();
					}
				}
				Graphics^ Rect = graphic_panel_P->CreateGraphics();
				Graphics^ Line =  graphic_panel_P->CreateGraphics();
				System::Drawing::Font^ drawFont = gcnew System::Drawing::Font( "Arial",12 );
				SolidBrush^ drawBrush = gcnew SolidBrush( Color::Black );
				
				for(int k=0; k < inst->C_Constant_parameters.M_get_n();k++)
				{			
					Rectangle drawRect = Rectangle(inst->Array[i][j][k]->M_get_x()+10,
						graphic_panel->Height-10-(inst->Array[i][j][k]->M_get_h() + inst->Array[i][j][k]->M_get_y()),
						inst->Array[i][j][k]->M_get_w(),inst->Array[i][j][k]->M_get_h());
					Rect->DrawRectangle(Pens::Black,drawRect);
					Rect->DrawString(inst->Array[i][j][k]->M_get_ID().ToString(), drawFont, drawBrush, drawRect );
					Line->DrawLine(Pens::Blue, 10, graphic_panel->Height-10 ,10, 10);
					Line->DrawLine(Pens::Red, 10 + inst->C_Constant_parameters.M_get_W(), 10 , 10 + inst->C_Constant_parameters.M_get_W(),
					graphic_panel->Height-10);
					Line->DrawLine(Pens::Blue, 10, graphic_panel->Height-10 ,graphic_panel->Width-10,graphic_panel->Height-10);
				}	
				//Rect->DrawString("Fin!", drawFont, drawBrush, Rectangle (10, graphic_panel->Height-10 - maxPackingHeight, 70, 70));
							
		}
private: System::Void btn_show_Click(System::Object^  sender, System::EventArgs^  e) 
		 {
			//Drow_Rectangles(graphic_panel);
			 Drow_Rectangles_1(graphic_panel);
		 }
private: System::Void button_reset_Click(System::Object^  sender, System::EventArgs^  e) {
			 button_reset->Enabled = false;	
			 class_Engine_App::getInstance()->~class_Engine_App();
			 button_start->Enabled = true;
			 button_show->Enabled = false;
		 }
private: System::Void btnOpenFirstPop_Click(System::Object^  sender, System::EventArgs^  e) 
		 {
			 // создаем новый диалог
			 OpenFileDialog^ openFileDialog1 = gcnew OpenFileDialog;
			 openFileDialog1->Filter = "The first population (*.txt)|*.txt";
			 openFileDialog1->FilterIndex = 1;
             openFileDialog1->RestoreDirectory = true;
			 if( openFileDialog1->ShowDialog() == System::Windows::Forms::DialogResult::OK )
			 {
				String ^f_name = openFileDialog1->FileName;									
				/*if( наш_класс.M_load_dataf((char*)(void*)Marshal::StringToHGlobalAnsi(f_name)) )	
					 Form1::label_status->Text = "Error opening the file! Error in markup.";
				else*/ Form1::label_status->Text = "Data file of the first pop.: " + openFileDialog1->SafeFileName;						
			 }
			 delete openFileDialog1;
		 }
};


}

