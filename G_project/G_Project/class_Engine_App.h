#pragma once
#include "class_Constant_parameters.h"
#include "class_Rectangle.h"
#include <map>

#define CROSS_TYPE_ONE 0  // One point crossover definition

#define MUTATION_MAX_1 0.10
#define MUTATION_MAX_2 0.10

#define MUTATION_MIN_1 0.02
#define MUTATION_MIN_2 0.02

#define GEN_W_MAX 100
#define GEN_H_MAX 100


#define CROSSOVER_DEBUG 1
#define MUTATION_DEBUG  1

#define LOG_FILE "logGenApp.txt"

//----------------------------------------------------------------------------------------------------------------
class class_Engine_App
{
	protected:
	static class_Engine_App* instance;
	private:
		
		int N;					// размер попул€ции (мощность начальной попул€ции)
		int type_of_decoder;	// тип декодера (0 - нижний левый, 1 - усовершенствованный нижний левый)
		int type_selection;		// тип селекции (0 - пропорциональна€, 1 - рангова€, 2 - турнирна€)
		int type_of_crossing;	// тип скрещивани€ (0 - одноточечное, 1 - двухточечное)
		int K_gen;				// коэффициент формировани€ нового поколени€ (0 - 0%, 100 - 100%)
		long spy_ID;			// следит за ID каждого пр€моугольника

		// критерии останова:
		short f_M[2];			// в каких поколени€х произошла ошибка
		short f_N[2];			// в каких списках произошла ошибка
		int finalGeneration;
		int bestPriorityListIndex;


	public:
		bool M_load_dataf(char *f_name);		// загрузка данных о первой попул€ции из файла

		// массивы:
		class_Rectangle **** Array;				// тройной массив указателей
		class_Rectangle *** ChildArray;			// Array of childs, is filled by the crossover.
		double *Ro;											// целева€ функци€
		class_Constant_parameters C_Constant_parameters;
		class_Rectangle ** input_rectangles;
		
		static class_Engine_App* getInstance();

		//Logging
		void WriteLogFile(System::String^ str);

		// главна€ функци€ программы
		bool M_main_func(int N_pop, int Dec, int Sel, int Cros, int K, int n, int W, int M, int joust);									// запуск осуществл€етс€ по кнопке "Start algorithm"
		void create_input_rectangles ();
		void create_new_generation(int new_generation_number);

		// критерии останова:
		bool M_comparison_of_two_prior_lists(int M_gen1, int M_gen2);
		bool M_comparison_of_two_prior_lists(int M_gen, int list1, int list2);

		int M_generation_first_population();

		// декодеры:
		void BLleft(class_Rectangle ****, int, int, int, bool *, int *);		//функци€ движени€ влево
		void BLbottom(class_Rectangle ****, int, int, int, bool *, int *);		//функци€ движени€ вниз
		void BLdecoder(class_Rectangle ****, int, int, int);					//декодер нижний левый

		void IBLleft(class_Rectangle ****, int, int, int, bool *, int, int *, int *, int *);		//движение влево(IBL)
		void IBLbottom(class_Rectangle ****, int, int, int, bool *, int *, int *, int *, int);		//движение вниз(IBL)
		void IBLdecoder(class_Rectangle ****, int, int, int);										//декодер усовершенствованный нижний левый
		
		//целева€ функци€:
		double target_function(class_Rectangle ****,int, int);

		// селекции:
		int M_proportional_selection(); // пропорциональна€ селекци€
		int M_rank_selection();			// рангова€ селекци€
		int M_tournament_selection();	// турнирна€ селекци€

        
		// получение закрытых членов класса:
		int M_get_N();
		int M_get_type_of_decoder();
		int M_get_type_selection();
		int M_get_type_of_crossing();
		int M_get_K_gen();
		int M_getFinalGenerationNumber();
		int M_getBestPriorityIdx();
		
		// методы с ID:
		long M_get_ID();				// получение ID
		int M_up_ID();					// увеличение ID
		int M_set_ID(long set_ID);		// установка ID
		
		// установка закрытых членов класса:
		int M_set_N(int set_N);
		int M_set_type_of_decoder(int set_type_of_decoder);
		int M_set_type_selection(int set_type_selection);
		int M_set_type_of_crossing(int set_type_of_crossing);
		int M_set_K_gen(int set_K_gen);
		int M_set_all(int set_N, int set_type_of_decoder, int set_type_selection, int set_type_of_crossing, int set_K_gen, long set_ID);

        //mutation block

		void M_mutationProcessOrder	(class_Rectangle **, int);
        void M_mutationProcessOrient (class_Rectangle **, int);
        void M_mutation	(class_Rectangle **, bool, int);

		// crossover block

		void M_createChildOnePoint (class_Rectangle **, class_Rectangle **, int, class_Rectangle***, int);
        void M_createChildTwoPoint (class_Rectangle **, class_Rectangle **, int, class_Rectangle***, int, int);
        void M_crossoverProcess (class_Rectangle **, class_Rectangle **, class_Constant_parameters & Uclass_Constant_parameters, int);


		protected:
		// конструкторы:
		class_Engine_App();
		// деструктор:
	public:
		~class_Engine_App();
};
//----------------------------------------------------------------------------------------------------------------