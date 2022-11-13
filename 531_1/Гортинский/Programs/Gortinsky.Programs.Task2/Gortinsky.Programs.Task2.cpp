#include <windows.h>
#include <stdio.h>
#include <iostream>
#include <fstream>

using namespace std;

int main(int argc, char** argv)
{
    setlocale(LC_ALL, "Russian");
    char secretSymbol = 'Q';

    BYTE diskData[512]{};
    DWORD bytesReadCount;
    HANDLE diskDescriptor = NULL;
    int sector = 0;

    TCHAR programFullName[MAX_PATH];
    GetModuleFileName(NULL, programFullName, MAX_PATH);
    TCHAR DriveLetter = programFullName[0];

    char driveName[7] = "\\\\.\\D:";
    driveName[4] = (char)programFullName[0];

    diskDescriptor = CreateFileA(driveName, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, NULL);                                  
    if (diskDescriptor == INVALID_HANDLE_VALUE)
    {
        printf("Неизвестная ошибка");
        return 1;
    }

    SetFilePointer(diskDescriptor, sector * 512, NULL, FILE_BEGIN);
    if (!ReadFile(diskDescriptor, diskData, 512, &bytesReadCount, NULL))
    {
        printf("Неизвестная ошибка");
        return 1;
    }

    auto fileName = "offset.txt";
    ifstream f(fileName);
    if (f.good())
    {
        fstream myfile(fileName, ios_base::in);

        int offset;
        while (myfile >> offset) {};

        for (int i = 0; i < 3; i++)
        {
            if (diskData[offset + i] != secretSymbol)
            {
                printf("Метка в разделе жесткого диска не найдена!");
                return 0;
            }
        }

        printf("Метка в разделе жесткого диска подтверждена");
        return 0;
    }

    int cntRow = 0;
    int address = -1;
    for (int i = 0; i < 512; i++)
    {
        if (diskData[i] == '\0')
            cntRow++;
        else
            cntRow = 0;

        if (cntRow == 5)
        {
            address = i - 3;
            break;
        }
    }

    if (address == -1)
    {
        printf("Неизвестная ошибка");
        return 1;
    }

    for (int i = 0; i < 3; i++)
    {
        diskData[address + i] = secretSymbol;
    }

    SetFilePointer(diskDescriptor, sector * 512, NULL, FILE_BEGIN);
    if (!WriteFile(diskDescriptor, diskData, 512, NULL, NULL))
    {
        printf("Неизвестная ошибка");
        return 1;
    }

    ofstream myfile;
    myfile.open(fileName);
    myfile << address;
    myfile.close();

    printf("Первый запуск. Метка в раздел жесткого диска успешно установлена");
    return 0;
}