
/*
 * Include Files
 *
 */
#if defined(MATLAB_MEX_FILE)
#include "tmwtypes.h"
#include "simstruc_types.h"
#else
#include "rtwtypes.h"
#endif



/* %%%-SFUNWIZ_wrapper_includes_Changes_BEGIN --- EDIT HERE TO _END */
#include <math.h>
/* %%%-SFUNWIZ_wrapper_includes_Changes_END --- EDIT HERE TO _BEGIN */
#define u_width 1
#define y_width 1

/*
 * Create external references here.  
 *
 */
/* %%%-SFUNWIZ_wrapper_externs_Changes_BEGIN --- EDIT HERE TO _END */
/* extern double func(double a); */
/* %%%-SFUNWIZ_wrapper_externs_Changes_END --- EDIT HERE TO _BEGIN */

/*
 * Output function
 *
 */
void u_Outputs_wrapper(const int32_T *u0,
			real_T *y0)
{
/* %%%-SFUNWIZ_wrapper_Outputs_Changes_BEGIN --- EDIT HERE TO _END */
double target[2];
    double direction[2];
    // me: 3 4, enemy: 15 16
    target[0] = u0[15] - u0[3];
    target[1] = u0[16] - u0[4];
    // w: 8
    direction[0] = cos(u0[8]/1000.0);
    direction[1] = sin(u0[8]/1000.0);
    // target dot direction / |target| |direction|
    double coserr = (target[0] * direction[0] + target[1] * direction[1])/sqrt(target[0] * target[0] + target[1] * target[1]);
    double err = acos(coserr);
    double z = target[0] * direction[1] - target[1] * direction[0];
    if(z < 0){
        err = -err;
    }
    *y0 = err * 100;
/* %%%-SFUNWIZ_wrapper_Outputs_Changes_END --- EDIT HERE TO _BEGIN */
}


