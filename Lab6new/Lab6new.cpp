#include "pch.h"
#include <omp.h>
#include <mpi.h>
#include <ctime>
#include <cstdlib>
#include <conio.h>
#include <iostream>

#define sendDataTag1 2000
#define sendDataTag2 2001
#define returnDataTag1 2002
#define returnDataTag2 2003
#define size 20000000

int main(int argc, char* argv[])
{
	int parallelMin, parallelMax, partialMin, partialMax, seqMin, seqMax;
	int rank, numRows, numRowsToSend, numProcessors, sender, numRowsToRecieve, avgRows, startRow, endRow;

	int i, seqStart, seqEnd, parStart, parEnd, count;
	int* vector = new int[size];
	
	srand(time(NULL));

	MPI_Status status;
	MPI_Init(&argc, &argv);


	MPI_Comm_rank(MPI_COMM_WORLD, &rank);
	MPI_Comm_size(MPI_COMM_WORLD, &numProcessors);

	int* localVector = new int[size / (numProcessors - 1) + numProcessors + 1];

	if (rank == 0)
	{
		for (i = 0; i < size; i++)
		{
			vector[i] = rand();
		}

		seqStart = clock();

		seqMin = vector[0];
		seqMax = vector[0];
		parallelMin = vector[0];
		parallelMax = vector[0];

		for (i = 0; i < size; i++)
		{
			if (vector[i] < seqMin)
				seqMin = vector[i];
			if (vector[i] > seqMax)
				seqMax = vector[i];
		}

		seqEnd = clock();
		
		std::cout << "Sequence time: " << (double)(seqEnd - seqStart) / CLOCKS_PER_SEC
			<< "\nmin: " << seqMin << " max: " << seqMax << std::endl;


		parStart = clock();

		avgRows = size / (numProcessors - 1);

		for (i = 1; i < numProcessors; i++) 
		{
			startRow = (i-1) * avgRows;
			endRow = i * avgRows  - 1;

			if ((size - endRow - 1) < avgRows)
				endRow = size - 1;

			numRowsToSend = endRow - startRow + 1;
			int* data = vector + startRow;
			MPI_Send(&numRowsToSend, 1, MPI_INT, i, sendDataTag1, MPI_COMM_WORLD);
			MPI_Send(data, numRowsToSend, MPI_INT, i, sendDataTag2, MPI_COMM_WORLD);
		}

		for (int i = 1; i < numProcessors; i++) {
			MPI_Recv(&partialMin, 1, MPI_INT, MPI_ANY_SOURCE, returnDataTag1, MPI_COMM_WORLD, &status);
			sender = status.MPI_SOURCE;
			MPI_Recv(&partialMax, 1, MPI_INT, MPI_ANY_SOURCE, returnDataTag2, MPI_COMM_WORLD, &status);
			sender = status.MPI_SOURCE;

			if (partialMin < parallelMin)
				parallelMin = partialMin;
			if (partialMax > parallelMax)
				parallelMax = partialMax;
		}

		parEnd = clock();

		std::cout << "Parallel time: " << (double)(parEnd - parStart) / CLOCKS_PER_SEC
			<< "\nmin: " << parallelMin << " max: " << parallelMax << std::endl;
		
		
	}

	if (rank != 0)
	{
		MPI_Recv(&numRowsToRecieve, 1, MPI_INT, 0, sendDataTag1, MPI_COMM_WORLD, &status);
		MPI_Recv(localVector, numRowsToRecieve, MPI_INT, 0, sendDataTag2, MPI_COMM_WORLD, &status);
		count = numRowsToRecieve;
		
		partialMin = localVector[0];
		partialMax = localVector[0];

		for (i = 0; i < count; i++)
		{
			if (localVector[i] < partialMin)
				partialMin = localVector[i];
			if (localVector[i] > partialMax)
				partialMax = localVector[i];
		}

		MPI_Send(&partialMin, 1, MPI_INT, 0, returnDataTag1, MPI_COMM_WORLD);
		MPI_Send(&partialMax, 1, MPI_INT, 0, returnDataTag2, MPI_COMM_WORLD);
	}
	MPI_Barrier(MPI_COMM_WORLD);
	MPI_Finalize();
}