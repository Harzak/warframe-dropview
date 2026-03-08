import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import { definePreset } from '@primeuix/themes';
import { routes } from './app.routes';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import Aura from '@primeuix/themes/aura';


const MyPreset = definePreset(Aura, {
  components: {
    select: {
      root: {
        background: 'var(--color-dark-black)',
        borderColor: 'var(--color-dark-grey)',
        color: 'var(--color-light-white)',
        hoverBorderColor: 'var(--color-light-white)',
        focusBorderColor: 'var(--color-light-white)',
      },
      option: {
        color: 'var(--color-light-white)',
        selectedColor: 'var(--color-light-white)',
        selectedFocusBackground: 'var(--color-lighter-green)',
        focusColor: 'var(--color-lighter-green)',
      },
      dropdown:{
        color: 'var(--color-light-white)',
      }
    },
    inputtext:{
      root: {
        background: 'var(--color-dark-black)',
        color: 'var(--color-light-white)',
        borderColor: 'var(--color-dark-grey)',
        focusBorderColor: 'var(--color-lighter-green)',
        placeholderColor: 'var(--color-light-grey)',
    },
  }
  }
});


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withInterceptors([errorInterceptor])),
    provideAnimationsAsync(),
    providePrimeNG({ theme: { preset: MyPreset } }),
  ],
};
