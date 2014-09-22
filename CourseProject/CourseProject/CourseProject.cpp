// CourseProject.cpp: ���������� ����� ����� ��� ����������.
#include "stdafx.h"
#include "CourseProject.h"
#include "stdexcept"
#include "lib.h"
#include "stdlib.h"
#include "stdio.h"
#include "windows.h"
#include "time.h"
#include "conio.h"
#include "string.h"
#include "sstream"
#include "iostream"

#define IDC_MAIN_BUTTON 101
#define MAX_LOADSTRING 100
#define file_W0 "d:/Study/W0.txt"
#define file_Wtek "d:/Study/Wtek.txt"
#define file_test "d:/Study/Test.txt"
#define K_svet 12					// ���������� ����������
#define K_sost 4					// ���������� ���������
#define Nx 7						// ����� ������ �� �����������
#define Ny 7						// ����� ������ �� ���������
#define dx 70						// ����� ������
#define dy 70						// ������ ������

using namespace lib;
using namespace std;

// ���������� ����������:
HINSTANCE hInst;								// ������� ���������
TCHAR szTitle[MAX_LOADSTRING];					// ����� ������ ���������
TCHAR szWindowClass[MAX_LOADSTRING];			// ��� ������ �������� ����

int I = -1, J = -1, I0 = 0, J0 = 0;				// ������ � ������� ������ �� ����
int x=0, y=0;									// ������� ���������� �������
Neuro_crossRoad* CrossroadArray[7][7];			// ������ ���������� �� �����������
int RoadArray[7][7];							// ������ �����
int chosenCrossroad = 0, chosenRoad = 0;		// ������� ��������� ��� �����������/������
int CrossroadID = 0;							// ���������� ������������ ������������ �� ����
FILE * Test;

// ���������� �������:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
void paintForm(HDC);
void printCrossroad(int x, int y, HDC hdc1);
void printCells(int X0, int Y0, int XCellsQuantity, int YCellsQuantity, HDC hdc1); 
void printRoad(int X0, int Y0, int roadType, HDC hdc1);
void DataExchange();
void getState();
void getPath(int i, int j, float currentData, int direction);


int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

 	// TODO: ���������� ��� �����.
	MSG msg;
	HACCEL hAccelTable;

	// ������������� ���������� �����
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_COURSEPROJECT, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// ��������� ������������� ����������:
	if (!InitInstance (hInstance, nCmdShow)) {
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_COURSEPROJECT));

	for (int i=0; i<Nx; i++) {
		for (int j=0; j<Ny; j++) { 
			CrossroadArray[i][j] = NULL;
			RoadArray[i][j] = 0;
		}
	}

	errno_t err;
	err = fopen_s(&Test, file_test, "rt");

	// ���� ��������� ���������:
	while (GetMessage(&msg, NULL, 0, 0)) {
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))	{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}
	return (int) msg.wParam;
}



//
//  �������: MyRegisterClass()
//  ����������: ������������ ����� ����.
//    ��� ������� � �� ������������� ���������� ������ � ������, ���� �����, ����� ������ ���
//    ��� ��������� � ��������� Win32, �� �������� ������� RegisterClassEx'
//    ������� ���� ��������� � Windows 95. ����� ���� ������� ����� ��� ����,
//    ����� ���������� �������� "������������" ������ ������ � ���������� ����� � ����.
//
ATOM MyRegisterClass(HINSTANCE hInstance) {
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_COURSEPROJECT));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_COURSEPROJECT);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   �������: InitInstance(HINSTANCE, int)
//   ����������: ��������� ��������� ���������� � ������� ������� ����.
//        � ������ ������� ���������� ���������� ����������� � ���������� ����������, � �����
//        ��������� � ��������� �� ����� ������� ���� ���������.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow) {
   HWND hWnd;
   hInst = hInstance; // ��������� ���������� ���������� � ���������� ����������
   hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, 800, 850, NULL, NULL, hInstance, NULL);
   if (!hWnd) {
      return FALSE;
   }
   ShowWindow(hWnd, nCmdShow);
   UpdateWindow(hWnd);
   return TRUE;
}

//
//  �������: WndProc(HWND, UINT, WPARAM, LPARAM)
//  ����������:  ������������ ��������� � ������� ����.
//  WM_COMMAND	- ��������� ���� ����������
//  WM_PAINT	-��������� ������� ����
//  WM_DESTROY	 - ������ ��������� � ������ � ���������.
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;
	static HDC hDC;

	switch (message) {
		case WM_COMMAND: // ������ ����:
			wmId    = LOWORD(wParam);
			wmEvent = HIWORD(wParam);
			
			switch (wmId) {
				case IDM_ABOUT:
					DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
					break;
				case IDM_EXIT:
					DestroyWindow(hWnd);
					break;
				default:
					return DefWindowProc(hWnd, message, wParam, lParam);
			}
			break;

		case WM_CREATE:
			hDC = GetDC(hWnd);
		    break;

		case WM_MOUSEMOVE:	//��������� �������� ����
	    	x = LOWORD(lParam);
	    	y = HIWORD(lParam);
	    	MoveToEx(hDC, x, y, NULL);
	    	J = (x-50)/dx;
	    	I = (y-50)/dy;
	    	paintForm(hDC);
	    	break;

		case WM_LBUTTONDOWN: 	// ��������� ������� (������� ����� ������ ����)
	    	x = LOWORD(lParam);
	    	y = HIWORD(lParam);
	    	MoveToEx(hDC, x, y, NULL);
		    J = (x-50)/dx;
		    I = (y-50)/dy;
		    paintForm(hDC);
		    break;

		case WM_LBUTTONUP:	//���������� ����� ������ ����
			if ( x >= 50 && y >= dy * Ny + 50 + 60 && x < 50 + dx*Nx && y < dy*Ny + 50+60+dy) {
			// �������� ����� �����������, ���� �� ��� �� �������
			    if (x >= 50 && y >= dy*Ny + 50 + 60 && x < 50 + dx && y < dy*Ny + 50+60+dy) {
			        chosenCrossroad = 1;
					chosenRoad = 0;
			    }
			}
		    if ( x >= 50 && y >= dy * Ny + 50 + 100 + dy && x < 50 + dx*Nx && y < dy*Ny + 50+100+dy*2) {
			// �������� ����� ������, ���� �� ��� �� �������
				for (int i=0; i<6; i++) {
					if (x >= 50+dx*i && y >= dy*Ny+50+100+dy && x < 50+dx*(i+1) && y < dy*Ny+50+100+dy*2) {
						chosenRoad = i+1;
						chosenCrossroad = 0;
						break;
					}
				}
			}

		    //���� ����� �������, �� �������� ����������� �� �����
			if ((I >= 0 && J >= 0 && I < Nx && J < Ny)) {
				if (CrossroadArray[I][J] == NULL && chosenCrossroad == 1 && RoadArray[I][J] == 0) {
					Enter_data * Data_CrossroadArray;
					Data_CrossroadArray=new Enter_data[12];
	
					//���������� ���. �� �����
					float l=0;
					if (!feof(Test) ) {
						for (int k=0;k<K_svet;k++) {
							fscanf(Test,"%f", &l);
							Data_CrossroadArray[k].Pk = l;
						}
					}
//�������� ������������ �����
//	wchar_t     szText2[250];
/*	swprintf(szText2,  L"����� 1=%f, 2=%f, 3=%f, 4=%f, 5=%f, 6=%f, 7=%f, 8=%f, 9=%f, 10=%f, 11=%f, 12=%f",  
		Data_CrossroadArray[0].Pk, Data_CrossroadArray[1].Pk, Data_CrossroadArray[2].Pk, Data_CrossroadArray[3].Pk,
		Data_CrossroadArray[4].Pk, Data_CrossroadArray[5].Pk, Data_CrossroadArray[6].Pk, Data_CrossroadArray[7].Pk,
		Data_CrossroadArray[8].Pk, Data_CrossroadArray[9].Pk, Data_CrossroadArray[10].Pk, Data_CrossroadArray[11].Pk 
		);
    MessageBox(NULL,  szText2,  L"ID",  MB_OK);*/
					CrossroadID = CrossroadID + 1;
					CrossroadArray[I][J] = new Neuro_crossRoad(K_svet, K_sost, file_W0, file_Wtek, Data_CrossroadArray, CrossroadID);
					//crossRoadVector.push_back(*CrossroadArray[I][J]);
/*
for(int k=0; k<CrossroadID;k++) {

    wchar_t     szText2[100]; 
    swprintf(szText2,  L"�������� ID = %d",  crossRoadVector[k].getID());
    MessageBox(NULL,  szText2,  L"ID",  MB_OK);
}	*/
				}
				if (CrossroadArray[I][J] == NULL && chosenRoad != 0 && RoadArray[I][J] == 0) {
					RoadArray[I][J] = chosenRoad;
				}
		    }
			paintForm(hDC);
		    break;

	case WM_RBUTTONUP:	// ��� ������� �� ������ ������ ���� ��������� ������� ������ ���������
		x = LOWORD(lParam);
	    y = HIWORD(lParam);
	    MoveToEx(hDC, x, y, NULL);
		J = (x-50)/dx;
		I = (y-50)/dy;
		RoadArray[I][J] = 0;
		CrossroadArray[I][J] = NULL;
		paintForm(hDC);
		break;

	case WM_KEYDOWN:  		// Enter - ������ ������� ����������
		switch (wParam) {
			case VK_RETURN:

				for(int i=0; i<Nx; i++) {
					for(int j=0; j<Ny; j++) {
						if(CrossroadArray[i][j] != NULL ) {
							for(int c=0; c<K_svet; c++) {	//����� �������� Ck ��� ������������ ���������� ����
								(*CrossroadArray[i][j]).D[c].Ck = 0;
							}
						}
					}
				}
				DataExchange();
				getState();	
/*	for(int i=0; i<Nx; i++) {
		for(int j=0; j<Ny; j++) {
			if(CrossroadArray[i][j] != NULL ) {
	wchar_t     szText2[250];
	swprintf(szText2,  L"ID = %d, ����� 1=%f, 2=%f, 3=%f, 4=%f, 5=%f, 6=%f, 7=%f, 8=%f, 9=%f, 10=%f, 11=%f, 12=%f",  
		(*CrossroadArray[i][j]).getID(), 
		(*CrossroadArray[i][j]).D[0].Ck, 
		(*CrossroadArray[i][j]).D[1].Ck, 
		(*CrossroadArray[i][j]).D[2].Ck,
		(*CrossroadArray[i][j]).D[3].Ck,
		(*CrossroadArray[i][j]).D[4].Ck,
		(*CrossroadArray[i][j]).D[5].Ck,
		(*CrossroadArray[i][j]).D[6].Ck,
		(*CrossroadArray[i][j]).D[7].Ck,
		(*CrossroadArray[i][j]).D[8].Ck,
		(*CrossroadArray[i][j]).D[9].Ck,
		(*CrossroadArray[i][j]).D[10].Ck,
		(*CrossroadArray[i][j]).D[11].Ck
		);
    MessageBox(NULL,  szText2,  L"ID",  MB_OK);
			}
		}
	}*/
				break;
		    }
		    break; 

	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		paintForm(hdc);
		EndPaint(hWnd, &ps);
		break;

	case WM_DESTROY:
		ReleaseDC(hWnd, hDC);
		PostQuitMessage(0);
		fclose(Test);
		break;

	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

void DataExchange() {
	float currentData = 0;		//��� ���������� �������� �������� CK

	for(int i=0; i<Nx; i++) {
		for(int j=0; j<Ny; j++) {
			if(CrossroadArray[i][j] != NULL ) {
				//��� ��������� �����
				if ((i>0) && (i<Nx-1) && (j>0) && (j<Ny-1)) { 
					//������� ������ ������ �� ���������������
					currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
					getPath(i-1, j, currentData, 10);
					//������� ������ ����� �� ���������������
					currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
					getPath(i, j-1, currentData, 11);
					//������� ������ ������ �� ���������������
					currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
					getPath(i, j+1, currentData, 12);
					//������� ������ ����� �� ���������������
					currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
					getPath(i+1, j, currentData, 13);
					//������� ������ ������ �� ���������������
/*					if (CrossroadArray[i-1][j] != NULL) { 
						currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
						getPath(i-1, j, currentData, 10);
					}
					if (RoadArray[i-1][j] != 0) {
						currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
						switch (RoadArray[i-1][j]) {
							case 2: 
								getPath(i-1, j, currentData, 10); 
								break;
							case 4:
								getPath(i-1, j-1, currentData, 11); 
								break;
							case 6:
	    						getPath(i-1, j+1, currentData, 12);
	    						break;
						}
					}
					//������� ������ ������ �� ���������������
					if (CrossroadArray[i][j+1] != NULL) { 
						currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
						getPath(i, j+1, currentData, 12);
					}
					if (RoadArray[i][j+1] != 0) {
						currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
						switch (RoadArray[i][j+1]) {
							case 1: 
								getPath(i, j+1, currentData, 12);
								break;
							case 3:
								getPath(i-1, j+1, currentData, 10);
								break;
							case 4:
	    						getPath(i+1, j+1, currentData, 13);
	    						break;
						}
					}
					//������� ������ ����� �� ���������������
					if (CrossroadArray[i+1][j] != NULL) { 
						currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
						getPath(i+1, j, currentData, 13);
					}
					if (RoadArray[i+1][j] != 0) {
						currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
						switch (RoadArray[i+1][j]) {
							case 2: 
								getPath(i+1, j, currentData, 13);
								break;
							case 3:
								getPath(i+1, j-1, currentData, 11);
								break;
							case 5:
	    						getPath(i+1, j+1, currentData, 12);
	    						break;
						}
					}
					//������� ������ ����� �� ���������������
					if (CrossroadArray[i][j-1] != NULL) { 
						currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
						getPath(i, j-1, currentData, 11);
					}
					if (RoadArray[i][j-1] != 0) {
						currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
						switch (RoadArray[i][j-1]) {
							case 1: 
								getPath(i, j-1, currentData, 11);
								break;
							case 5:
								getPath(i-1, j-1, currentData, 10);
								break;
							case 6:
	    						getPath(i+1, j-1, currentData, 13);
	    						break;
						}
					}*/
					continue;
				}
				//��� ����� ������� ������
				if (i==0) { 
					if (j == 0) {
						//������� ������ ������ �� ���������������
						currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
						getPath(i, j+1, currentData, 12);
						//������� ������ ����� �� ���������������
						currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
						getPath(i+1, j, currentData, 13);
						continue;
					} else {
						if (j == Ny-1) {
							//������� ������ ����� �� ���������������
							currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
							getPath(i, j-1, currentData, 11);
							//������� ������ ����� �� ���������������
							currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
							getPath(i+1, j, currentData, 13);
							continue;
						} else {
							//������� ������ ����� �� ���������������
							currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
							getPath(i, j-1, currentData, 11);
							//������� ������ ������ �� ���������������
							currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
							getPath(i, j+1, currentData, 12);
							//������� ������ ����� �� ���������������
							currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
							getPath(i+1, j, currentData, 13);
							continue;
						}
					}
				}
				//��� ����� ������ ������
				if (i==Nx-1) { 
					if (j == 0) {
						//������� ������ ������ �� ���������������
						currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
						getPath(i-1, j, currentData, 10);
						//������� ������ ������ �� ���������������
						currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
						getPath(i, j+1, currentData, 12);
						continue;
					} else {
						if (j == Ny-1) {
							//������� ������ ������ �� ���������������
							currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
							getPath(i-1, j, currentData, 10);
							//������� ������ ����� �� ���������������
							currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
							getPath(i, j-1, currentData, 11);
							continue;
						} else {
							//������� ������ ������ �� ���������������
							currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
							getPath(i-1, j, currentData, 10);
							//������� ������ ����� �� ���������������
							currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
							getPath(i, j-1, currentData, 11);
							//������� ������ ������ �� ���������������
							currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
							getPath(i, j+1, currentData, 12);
							continue;
						}
					}
				}
				//��� ����� ������� ����(����� �������)
				if ((i>0) && (i<Nx-1) && (j == 0)) { 
					//������� ������ ������ �� ���������������
					currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
					getPath(i-1, j, currentData, 10);
					//������� ������ ������ �� ���������������
					currentData=((*CrossroadArray[i][j]).D[6].Pk+(*CrossroadArray[i][j]).D[7].Pk+(*CrossroadArray[i][j]).D[8].Pk)/3;
					getPath(i, j+1, currentData, 12);
					//������� ������ ����� �� ���������������
					currentData=((*CrossroadArray[i][j]).D[3].Pk+(*CrossroadArray[i][j]).D[4].Pk+(*CrossroadArray[i][j]).D[5].Pk)/3;
					getPath(i+1, j, currentData, 13);
					continue;
				}
				//��� ����� �������� ������� ����(����� �������)
				if ((i>0) && (i<Ny-1) && (j == Nx-1)) { 
					//������� ������ ������ �� ���������������
					currentData=((*CrossroadArray[i][j]).D[9].Pk+(*CrossroadArray[i][j]).D[10].Pk+(*CrossroadArray[i][j]).D[11].Pk)/3;
					getPath(i-1, j, currentData, 10);
					//������� ������ ����� �� ���������������
					currentData=((*CrossroadArray[i][j]).D[0].Pk+(*CrossroadArray[i][j]).D[1].Pk+(*CrossroadArray[i][j]).D[2].Pk)/3;
					getPath(i, j-1, currentData, 11);
					//������� ������ ����� �� ���������������
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
	float* resultArr;
	resultArr = new float[K_sost];
	for(int i=0; i<K_sost; i++) {
		resultArr[i] = 0;
	}
	for(int i=0; i<Nx; i++) {
		for(int j=0; j<Ny; j++) {
			if(CrossroadArray[i][j] != NULL ) {
				resultArr = (*(CrossroadArray[i][j])).Neuro_crossRoad_Step((*(CrossroadArray[i][j])).D);
				/*wchar_t     szText2[250];
				swprintf(szText2,  L"ID = %d, ����� 1=%f, 2=%f, 3=%f, 4=%f",  (*(CrossroadArray[i][j])).getID(), 
					resultArr[0], resultArr[1], resultArr[2], resultArr[3]);
				MessageBox(NULL,  szText2,  L"Res",  MB_OK);*/

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

				wchar_t     szText2[250];
				swprintf(szText2,  L" Results: ID = %d, ����� 1=%1.0f, 2=%1.0f, 3=%1.0f, 4=%1.0f \n Optimal state - %d", 
					(*(CrossroadArray[i][j])).getID(), resultArr[0], resultArr[1], resultArr[2], resultArr[3], max_ind + 1);
				MessageBox(NULL,  szText2,  L"Results",  MB_OK);
			}
		}
	}
}

// �������������� ���������� ������� ����
void paintForm(HDC hdc1) {
	HBRUSH b1, b2;
	b1 = (HBRUSH)GetStockObject(WHITE_BRUSH);
	b2 = (HBRUSH)GetStockObject(LTGRAY_BRUSH);
	static int x1 = 0, x2 = 0, y1 = 0, y2 = 0;	// ��� ��������� ��������� ������

	printCells(50, 50, Nx, Ny, hdc1); //���������� ������ ���� ����������
	printCells(50, dy*Ny + 50 + 60, Nx, 1, hdc1); //���������� ������ � �������� ������������
	printCells(50, dy*Ny + 50 + 100 + dy, Nx, 1, hdc1); //���������� ������ � �������� �����

	if ( I >= 0 && J >= 0 && I < Ny && J < Nx) {  //������������ ����� ������ ������, �� ������� ��������� ������
			SelectObject(hdc1,b1);
			Rectangle(hdc1,x1,y1,x2,y2);
			x1 = 50 + dx * J;
			y1 = 50 + dy * I;
			x2 = 51 + dx * (J+1);
			y2 = 51 + dy * (I+1);
			SelectObject(hdc1,b2);
			Rectangle(hdc1,x1,y1,x2,y2);
	} else {
			SelectObject(hdc1,b1);
			Rectangle(hdc1,x1,y1,x2,y2);
	}

	/*for (int h=1; h<=6; h++) {
		if (chosenRoad == h) {
			SelectObject(hdc1, b2);
			Rectangle(hdc1, 50+dx*(h-1), dy*Ny + 50 + 100 + dy, 51+dx*h, dy*Ny + 51 + 100 + dy*2);
		}
		else {
			SelectObject(hdc1,b1);
			Rectangle(hdc1, 50+dx*(h-1), dy*Ny + 50 + 100 + dy, 51+dx*h, dy*Ny + 51 + 100 + dy*2);
		}
	}
	if (chosenCrossroad == 1) {
		SelectObject(hdc1, b2);
		Rectangle(hdc1, 50, dy*Ny + 50 + 60, 51+dx, dy*Ny + 51 + 60 + dy);
	}*/

	//���������� ������������ �� ���� ����������
	for (int h=0; h<Nx; h++) {
		for (int v=0; v<Ny; v++) { 
			if (CrossroadArray[h][v] != 0) {
				printCrossroad((50+v*dx), (50+h*dy), hdc1);
			}
			if (RoadArray[h][v] != 0) {
				printRoad((50+v*dx), (50+h*dy), RoadArray[h][v], hdc1);
			}
		}
	}
	
	//���������� ������ �����������
	printCrossroad(50, dy*Ny + 50 + 60, hdc1);
	// ���������� ������� �����
	for(int h=0; h<6; h++) {
		printRoad(50+dx*h, dy*Ny + 50 + 100 + dy, h+1, hdc1);
	}

	static LOGFONT lf;
	memset(&lf, 0, sizeof(LOGFONT));
	lf.lfPitchAndFamily = FIXED_PITCH | FF_MODERN;
	lf.lfCharSet = DEFAULT_CHARSET;
	lf.lfItalic = FALSE;
	lf.lfWeight = FW_BOLD;
	lf.lfHeight = 30;
	HFONT hFont1 = CreateFontIndirect(&lf);
	SelectObject(hdc1, hFont1);		
    SetTextColor(hdc1, RGB(200, 200, 200));

    //����� ��������
	TextOut(hdc1, (50), (20), _T("���� ����������"), strlen("���� ����������"));
	TextOut(hdc1, (50), (dy*Ny + 50 + 30), _T("���� ������������"), strlen("���� ������������"));
	TextOut(hdc1, (50), (dy*Ny + 50 +70 +dy), _T("���� �����"), strlen("���� �����"));

	DeleteObject(b1);
	DeleteObject(b2);

	return;
}

void printCells(int X0, int Y0, int XCellsQuantity, int YCellsQuantity, HDC hdc1) {
	for(int i=0; i<YCellsQuantity+1; i++) { //���������� �������������� �����
		MoveToEx(hdc1, X0, Y0+i*dy, NULL);  //������ ���� � ������ �����
		LineTo(hdc1, X0+XCellsQuantity*dx, Y0+i*dy);
	}
	for(int j=0; j<XCellsQuantity+1; j++) { //���������� ������������ �����
		MoveToEx(hdc1, X0+j*dx, Y0, NULL);  //������ ���� � ������ �����
		LineTo(hdc1, X0+j*dx, Y0+YCellsQuantity*dy);
	}
}

void printCrossroad(int x, int y, HDC hdc1) {
	for(int i=0; i<3; i++)  //���������� �������������� ����� 
	{	
		MoveToEx(hdc1, x, (y+20+5*i), NULL); 
		LineTo(hdc1, (x+20+5*i), (y+20+5*i));
		MoveToEx(hdc1, (x+35+5*(3-i)), (y+20+5*i), NULL); 
		LineTo(hdc1, (x+70), (y+20+5*i));
	}
	for(int i=0; i<3; i++)  //���������� �������������� ����� 
	{	
		MoveToEx(hdc1, x, (y+40+5*i), NULL);  
		LineTo(hdc1, (x+20+5*(2-i)), (y+40+5*i));
		MoveToEx(hdc1, (x+40+5*i), (y+40+5*i), NULL); 
		LineTo(hdc1, (x+70), (y+40+5*i));
	}
	
	for(int j=0; j<3; j++)  //���������� ������������ �����
	{
		MoveToEx(hdc1, (x+20+5*j), y, NULL);  
		LineTo(hdc1, (x+20+5*j), (y+20+5*j));
		MoveToEx(hdc1, (x+20+5*j), (y+40+5*(2-j)), NULL);  
		LineTo(hdc1, (x+20+5*j), (y+70));
	}		
	for(int j=0; j<3; j++)  //���������� ������������ �����
	{
		MoveToEx(hdc1, (x+40+5*j), y, NULL);  
		LineTo(hdc1, (x+40+5*j), (y+20+5*(2-j)));
		MoveToEx(hdc1, (x+40+5*j), (y+40+5*j), NULL);  
		LineTo(hdc1, (x+40+5*j), (y+70));
	}
	//����������� ������
	MoveToEx(hdc1, (x+35), y, NULL);  
	LineTo(hdc1, (x+35), (y+70));
	MoveToEx(hdc1, x, (y+35), NULL); 
	LineTo(hdc1, (x+70), (y+35));
}

void printRoad(int X0, int Y0, int roadType, HDC hdc1) {
	switch (roadType) {
		case 1:
			for(int i=0; i<7; i++) { //���������� �������������� �����
				MoveToEx(hdc1, X0, (Y0+20+5*i), NULL);  //������ ���� � ������ �����
				LineTo(hdc1, (X0+dx), (Y0+20+5*i));
			}
			break;
		case 2:
			for(int j=0; j<7; j++) { //���������� ������������ �����
				MoveToEx(hdc1, (X0+20+5*j), Y0, NULL);  //������ ���� � ������ �����
				LineTo(hdc1, (X0+20+5*j), (Y0+dy));
			}	
			break;
		case 3:
			for(int i=0; i<7; i++) { //���������� �������������� ����� 	
				MoveToEx(hdc1, X0, (Y0+20+5*i), NULL);  
				LineTo(hdc1, (X0+20+5*i), (Y0+20+5*i));
			}
			for(int j=0; j<7; j++) { //���������� ������������ �����
				MoveToEx(hdc1, (X0+20+5*j), Y0, NULL);  
				LineTo(hdc1, (X0+20+5*j), (Y0+20+5*j));
			}
			break;
		case 4:
			for(int i=0; i<7; i++) { //���������� �������������� ����� 	
				MoveToEx(hdc1, X0, (Y0+20+5*i), NULL);  
				LineTo(hdc1, (X0+20+5*(6-i)), (Y0+20+5*i));
			}
			for(int j=0; j<7; j++) { //���������� ������������ �����
				MoveToEx(hdc1, (X0+20+5*j), (Y0+20+5*(6-j)), NULL);  
				LineTo(hdc1, (X0+20+5*j), (Y0+dy));
			}
			break;
		case 5:
			for(int i=0; i<7; i++) { //���������� �������������� ����� 	
				MoveToEx(hdc1, (X0+20+5*i), (Y0+20+5*(6-i)), NULL);  
				LineTo(hdc1, (X0+dx), (Y0+20+5*(6-i)));
			}
			for(int j=0; j<7; j++) { //���������� ������������ ����� 	
				MoveToEx(hdc1, (X0+20+5*j), Y0, NULL);  
				LineTo(hdc1, (X0+20+5*j), (Y0+20+5*(6-j)));
			}
			break;
		case 6:
			for(int i=0; i<7; i++) { //���������� �������������� ����� 	
				MoveToEx(hdc1, (X0+20+5*i), (Y0+20+5*i), NULL);  
				LineTo(hdc1, (X0+dx), (Y0+20+5*i));
			}
			for(int j=0; j<7; j++) { //���������� ������������ ����� 	
				MoveToEx(hdc1, (X0+20+5*j), (Y0+20+5*j), NULL);  
				LineTo(hdc1, (X0+20+5*j), (Y0+dy));
			}	
			break;
	}
}

// ���������� ��������� ��� ���� "� ���������".
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
