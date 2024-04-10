import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SseService } from './sse.service';

@Component({
  selector: 'sse-component',
  template: `
    <div *ngFor="let notification of notifications">
      <p>{{ notification }}</p>
    </div>
  `
})
export class SseComponent  {

  notifications: string[] = [];

  constructor(private sseService: SseService, private cdr: ChangeDetectorRef) { }

}

  // ngOnInit(): void {
  //   this.sseService.connectToSse('asdf').subscribe({
  //     next : data => {
  //       console.log("CONECTED");
  //       console.log(data);

  //       this.notifications.push(data);
  //       this.cdr.detectChanges();
  //     },
  //     error: error => {
  //       console.log("CONECTED ERROR");
  //       console.error('Error in SSE connection:', error);
  //     }
  //   });
    // this.sseService.connectToSse().subscribe(
    //   data => {
    //     this.notifications.push(data);
    //   },
    //   error => {
    //     console.error('Error in SSE connection:', error);
    //   }
    // );
  //}

  // ngOnDestroy(): void {
  //   // this.sseService.closeConnection();
  // }

