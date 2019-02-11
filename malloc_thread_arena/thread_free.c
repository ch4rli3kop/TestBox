/* this test was made for identifying 
if each threads has arena, and share freed chunk.
*/
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <unistd.h>
#include <assert.h>

void *thread_funcn(void* name){
	pid_t pid = getpid();
	pthread_t tid = pthread_self();

	printf("thread name is %s\n", (char*)name);
	int* a = malloc(0x60);
	printf("new malloc address : %p\n",a);
	free(a);
	printf("%p was freed\n",a);
	sleep(1);

}

void main(){
	pthread_t p_thread;
	int thread_id;
	int status;

	char p1[] = "thread 1";
	char p2[] = "thread 2";

	sleep(1);

	thread_id = pthread_create(&p_thread, NULL, &thread_funcn, (void*)p1);
	if (thread_id < 0){
		perror("thread create error!\n");
		exit(0);
	}

	thread_funcn((void*)p2);
	pthread_join(p_thread, (void**)&status);
	printf("finished\n");
}