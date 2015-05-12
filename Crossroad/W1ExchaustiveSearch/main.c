#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <math.h>
#include <omp.h>
#include <conio.h>

#define NOF_TRAFFIC_LIGHTS        (12)
#define NOF_STATES                (4)
#define PATH_TO_EDUCATION_EXAMPLE ("education_example.txt")
#define NOF_EDUCATION_EXAMPLES    (192)
#define EPSILON                   (0.0000001)

void read_education_example(double **education_examples)
{
	int i = 0;
	FILE *f = fopen(PATH_TO_EDUCATION_EXAMPLE, "r");

	for (i = 0; i < NOF_EDUCATION_EXAMPLES; i++)
	{
		fscanf(f, "%lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf",
		   education_examples[i], education_examples[i] + 1, education_examples[i] + 2,
		   education_examples[i] + 3, education_examples[i] + 4, education_examples[i] + 5,
		   education_examples[i] + 6, education_examples[i] + 7, education_examples[i] + 8,
		   education_examples[i] + 9, education_examples[i] + 10, education_examples[i] + 11,
		   education_examples[i] + 12, education_examples[i] + 13, education_examples[i] + 14,
		   education_examples[i] + 15);
	}
	
	fclose(f);
}

void init_rand_w1(double **w1)
{
	int i, j;
	double rand_max = RAND_MAX;

	srand(time(NULL));

	for (i = 0; i < NOF_TRAFFIC_LIGHTS; i++)
	{
		for (j = 0; j < NOF_STATES; j++)
		{
			w1[i][j] = rand() / rand_max - 0.5;
		}
	}
}

void init_minone_w1(double **w1)
{
	int i, j;

	for (i = 0; i < NOF_TRAFFIC_LIGHTS; i++)
	{
		for (j = 0; j < NOF_STATES; j++)
		{
			w1[i][j] = -1.0;
		}
	}
}

double activate(double *dendrits)
{
	int i;
	double a;
	double sum = 0.0;
	for (i = 0; i < NOF_TRAFFIC_LIGHTS; i++)
	{
		sum += dendrits[i];
	}

	a = atan(sum);
	return a >= 0 ? 1 : 0;
}

double compute_pass_rate(double **education_examples, double **w1)
{
	int i, j, k;
	double pass_rate = 0.0;

	for (k = 0; k < NOF_EDUCATION_EXAMPLES; k++)
	{
		char is_passed = 1;
		for (i = 0; i < NOF_STATES; i++)
		{
			double a;
			double dendrits[NOF_TRAFFIC_LIGHTS];
			for (j = 0; j < NOF_TRAFFIC_LIGHTS; j++)
			{
				dendrits[j] = education_examples[k][j] * w1[j][i];
			}

			a = activate(dendrits);
			if (abs(a - education_examples[k][NOF_TRAFFIC_LIGHTS + i] - -1) < EPSILON)
			{
				is_passed = 0;
				break;
			}
			else if (abs(a - education_examples[k][NOF_TRAFFIC_LIGHTS + i] - 1) < EPSILON)
			{
				is_passed = 0;
				break;
			}
		}

		if (is_passed)
		{
			pass_rate += 1.0;
		}
	}

	return pass_rate / NOF_EDUCATION_EXAMPLES;
}

int main()
{
	/*int i, j;
	double **w1;
	double **education_examples;
	double step = 0.0001;
	double best_pass_rate = 0.0;

	w1 = malloc(NOF_TRAFFIC_LIGHTS * sizeof(double*));
	for (i = 0; i < NOF_TRAFFIC_LIGHTS; i++)
	{
		w1 = malloc(NOF_STATES * sizeof(double));
	}

	education_examples = malloc(NOF_EDUCATION_EXAMPLES * sizeof(double*));
	for (i = 0; i < NOF_EDUCATION_EXAMPLES; i++)
	{
		education_examples[i] = malloc((NOF_TRAFFIC_LIGHTS + NOF_STATES) * sizeof(double));
	}

	read_education_example(education_examples);
	init_minone_w1(w1);

	for (i = 0; i < NOF_TRAFFIC_LIGHTS; i++)
	{
		for (j = 0; j < NOF_STATES; j++)
		{
			while (w1[i][j] <= 1)
			{
				double pass_rate = compute_pass_rate(education_examples, w1);
				if (pass_rate > best_pass_rate)
				{
					best_pass_rate = pass_rate;
				}

				w1[i][j] += step;
			}
		}
	}*/

	int i = 0;
	double from = -1;
	double to = 1;
	double step = 0.5;
	double education_examples[NOF_EDUCATION_EXAMPLES * (NOF_TRAFFIC_LIGHTS + NOF_STATES)];

	FILE *f = fopen(PATH_TO_EDUCATION_EXAMPLE, "r");

	for (i = 0; i < NOF_EDUCATION_EXAMPLES; i++)
	{
		int base_ind = i * (NOF_TRAFFIC_LIGHTS + NOF_STATES);
		fscanf(f, "%lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf %lf",
		   education_examples + base_ind, education_examples + base_ind + 1, education_examples + base_ind + 2,
		   education_examples + base_ind + 3, education_examples + base_ind + 4, education_examples + base_ind + 5,
		   education_examples + base_ind + 6, education_examples + base_ind + 7, education_examples + base_ind + 8,
		   education_examples + base_ind + 9, education_examples + base_ind + 10, education_examples + base_ind + 11,
		   education_examples + base_ind + 12, education_examples + base_ind + 13, education_examples + base_ind + 14,
		   education_examples + base_ind + 15);
	}
	
	fclose(f);

	omp_set_num_threads(8);

#pragma omp parallel shared(from,to,step,education_examples)
	{
		double w_0, w_1, w_2, w_3, w_4, w_5, w_6, w_7, w_8, w_9, w_10;
		double w_11, w_12, w_13, w_14, w_15, w_16, w_17, w_18, w_19, w_20, w_21;
		double w_22, w_23, w_24, w_25, w_26, w_27, w_28, w_29, w_30, w_31, w_32;
		double w_33, w_34, w_35, w_36, w_37, w_38, w_39, w_40, w_41, w_42, w_43;
		double w_44, w_45, w_46, w_47;
		double bw_0, bw_1, bw_2, bw_3, bw_4, bw_5, bw_6, bw_7, bw_8, bw_9, bw_10;
		double bw_11, bw_12, bw_13, bw_14, bw_15, bw_16, bw_17, bw_18, bw_19, bw_20, bw_21;
		double bw_22, bw_23, bw_24, bw_25, bw_26, bw_27, bw_28, bw_29, bw_30, bw_31, bw_32;
		double bw_33, bw_34, bw_35, bw_36, bw_37, bw_38, bw_39, bw_40, bw_41, bw_42, bw_43;
		double bw_44, bw_45, bw_46, bw_47;
		double best_pass_rate = 0.0;
		int tid;

		for (w_0 = from; w_0 <= to; w_0 += step)
		for (w_1 = from; w_1 <= to; w_1 += step)
		for (w_2 = from; w_2 <= to; w_2 += step)	
		for (w_3 = from; w_3 <= to; w_3 += step)
		for (w_4 = from; w_4 <= to; w_4 += step)
		for (w_5 = from; w_5 <= to; w_5 += step)
		for (w_6 = from; w_6 <= to; w_6 += step)
		for (w_7 = from; w_7 <= to; w_7 += step)
		for (w_8 = from; w_8 <= to; w_8 += step)
		for (w_9 = from; w_9 <= to; w_9 += step)
		for (w_10 = from; w_10 <= to; w_10 += step)
		for (w_11 = from; w_11 <= to; w_11 += step)
		for (w_12 = from; w_12 <= to; w_12 += step)
		for (w_13 = from; w_13 <= to; w_13 += step)
		for (w_14 = from; w_14 <= to; w_14 += step)
		for (w_15 = from; w_15 <= to; w_15 += step)
		for (w_16 = from; w_16 <= to; w_16 += step)
		for (w_17 = from; w_17 <= to; w_17 += step)
		for (w_18 = from; w_18 <= to; w_18 += step)
		for (w_19 = from; w_19 <= to; w_19 += step)
		for (w_20 = from; w_20 <= to; w_20 += step)
		for (w_21 = from; w_21 <= to; w_21 += step)
		for (w_22 = from; w_22 <= to; w_22 += step)
		for (w_23 = from; w_23 <= to; w_23 += step)
		for (w_24 = from; w_24 <= to; w_24 += step)
		for (w_25 = from; w_25 <= to; w_25 += step)
		for (w_26 = from; w_26 <= to; w_26 += step)
		for (w_27 = from; w_27 <= to; w_27 += step)
		for (w_28 = from; w_28 <= to; w_28 += step)
		for (w_29 = from; w_29 <= to; w_29 += step)
		for (w_30 = from; w_30 <= to; w_30 += step)
		for (w_31 = from; w_31 <= to; w_31 += step)
		for (w_32 = from; w_32 <= to; w_32 += step)
		for (w_33 = from; w_33 <= to; w_33 += step)
		for (w_34 = from; w_34 <= to; w_34 += step)
		for (w_35 = from; w_35 <= to; w_35 += step)
		for (w_36 = from; w_36 <= to; w_36 += step)
		for (w_37 = from; w_37 <= to; w_37 += step)
		for (w_38 = from; w_38 <= to; w_38 += step)
		for (w_39 = from; w_39 <= to; w_39 += step)
		for (w_40 = from; w_40 <= to; w_40 += step)
		for (w_41 = from; w_41 <= to; w_41 += step)
		for (w_42 = from; w_42 <= to; w_42 += step)
		for (w_43 = from; w_43 <= to; w_43 += step)
		for (w_44 = from; w_44 <= to; w_44 += step)
		for (w_45 = from; w_45 <= to; w_45 += step)
		for (w_46 = from; w_46 <= to; w_46 += step)
		for (w_47 = from; w_47 <= to; w_47 += step)
		{
			int j, k;
			double pass_rate = 0.0;

			for (k = 0; k < NOF_EDUCATION_EXAMPLES; k++)
			{
				double dendrit_0 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 0] * w_0;
				double dendrit_1 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 1] * w_1;
				double dendrit_2 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 2] * w_2;
				double dendrit_3 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 3] * w_3;
				double dendrit_4 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 4] * w_4;
				double dendrit_5 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 5] * w_5;
				double dendrit_6 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 6] * w_6;
				double dendrit_7 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 7] * w_7;
				double dendrit_8 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 8] * w_8;
				double dendrit_9 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 9] * w_9;
				double dendrit_10 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 10] * w_10;
				double dendrit_11 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 11] * w_11;

				double sum = dendrit_0 + dendrit_1 + dendrit_2 + dendrit_3 + dendrit_4 + dendrit_5 + dendrit_6 +
					dendrit_7 + dendrit_8 + dendrit_9 + dendrit_10 + dendrit_11;

				double a = atan(sum);
				if (a > 0) a = 1;
				else a = 0;

				if (abs(a - education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 12] - -1) < EPSILON)
				{
					continue;
				}
				else if (abs(a - education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 12] - 1) < EPSILON)
				{
					continue;
				}

				dendrit_0 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 0] * w_12;
				dendrit_1 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 1] * w_13;
				dendrit_2 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 2] * w_14;
				dendrit_3 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 3] * w_15;
				dendrit_4 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 4] * w_16;
				dendrit_5 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 5] * w_17;
				dendrit_6 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 6] * w_18;
				dendrit_7 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 7] * w_19;
				dendrit_8 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 8] * w_20;
				dendrit_9 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 9] * w_21;
				dendrit_10 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 10] * w_22;
				dendrit_11 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 11] * w_23;

				sum = dendrit_0 + dendrit_1 + dendrit_2 + dendrit_3 + dendrit_4 + dendrit_5 + dendrit_6 +
					dendrit_7 + dendrit_8 + dendrit_9 + dendrit_10 + dendrit_11;

				a = atan(sum);
				if (a > 0) a = 1;
				else a = 0;

				if (abs(a - education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 13] - -1) < EPSILON)
				{
					continue;
				}
				else if (abs(a - education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 13] - 1) < EPSILON)
				{
					continue;
				}

				dendrit_0 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 0] * w_24;
				dendrit_1 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 1] * w_25;
				dendrit_2 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 2] * w_26;
				dendrit_3 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 3] * w_27;
				dendrit_4 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 4] * w_28;
				dendrit_5 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 5] * w_29;
				dendrit_6 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 6] * w_30;
				dendrit_7 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 7] * w_31;
				dendrit_8 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 8] * w_32;
				dendrit_9 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 9] * w_33;
				dendrit_10 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 10] * w_34;
				dendrit_11 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 11] * w_35;

				sum = dendrit_0 + dendrit_1 + dendrit_2 + dendrit_3 + dendrit_4 + dendrit_5 + dendrit_6 +
					dendrit_7 + dendrit_8 + dendrit_9 + dendrit_10 + dendrit_11;

				a = atan(sum);
				if (a > 0) a = 1;
				else a = 0;

				if (abs(a - education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 14] - -1) < EPSILON)
				{
					continue;
				}
				else if (abs(a - education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 14] - 1) < EPSILON)
				{
					continue;
				}

				dendrit_0 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 0] * w_36;
				dendrit_1 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 1] * w_37;
				dendrit_2 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 2] * w_38;
				dendrit_3 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 3] * w_39;
				dendrit_4 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 4] * w_40;
				dendrit_5 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 5] * w_41;
				dendrit_6 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 6] * w_42;
				dendrit_7 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 7] * w_43;
				dendrit_8 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 8] * w_44;
				dendrit_9 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 9] * w_45;
				dendrit_10 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 10] * w_46;
				dendrit_11 = education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 11] * w_47;

				sum = dendrit_0 + dendrit_1 + dendrit_2 + dendrit_3 + dendrit_4 + dendrit_5 + dendrit_6 +
					dendrit_7 + dendrit_8 + dendrit_9 + dendrit_10 + dendrit_11;

				a = atan(sum);
				if (a > 0) a = 1;
				else a = 0;

				if (abs(a - education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 15] - -1) < EPSILON)
				{
					continue;
				}
				else if (abs(a - education_examples[k * (NOF_TRAFFIC_LIGHTS + NOF_STATES) + 15] - 1) < EPSILON)
				{
					continue;
				}

				pass_rate += 1.0;
			}

			pass_rate /= NOF_EDUCATION_EXAMPLES;

			if (pass_rate > best_pass_rate)
			{
				best_pass_rate = pass_rate;
				bw_0 = w_0;bw_1 = w_1;bw_2 = w_2;bw_3 = w_3;bw_4 = w_4;bw_5 = w_5;bw_6 = w_6;bw_7 = w_7;bw_8 = w_8;bw_9 = w_9;bw_10 = w_10;bw_11 = w_11;bw_12 = w_12;bw_13 = w_13;bw_14 = w_14;bw_15 = w_15;bw_16 = w_16;bw_17 = w_17;bw_18 = w_18;bw_19 = w_19;bw_20 = w_20;bw_21 = w_21;bw_22 = w_22;bw_23 = w_23;bw_24 = w_24;bw_25 = w_25;bw_26 = w_26;bw_27 = w_27;bw_28 = w_28;bw_29 = w_29;bw_30 = w_30;bw_31 = w_31;bw_32 = w_32;bw_33 = w_33;bw_34 = w_34;bw_35 = w_35;bw_36 = w_36;bw_37 = w_37;bw_38 = w_38;bw_39 = w_39;bw_40 = w_40;bw_41 = w_41;bw_42 = w_42;bw_43 = w_43;bw_44 = w_44;bw_45 = w_45;bw_46 = w_46;bw_47 = w_47;
			}
		}

#pragma omp critical
		{
			tid = omp_get_thread_num();
			printf("thread %d best pass rate is %lf", tid, best_pass_rate);
		}
	}

	getch();

	return 0;
}