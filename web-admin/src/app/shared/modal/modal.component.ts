import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
})
export class ModalComponent {

  @ViewChild('exampleModal') modalElementRef!: ElementRef;
  @Input() modalType: string = 'information';
  @Input() title: string = '';
  @Input() message: string = '';

  @Output() onAccept = new EventEmitter();
  @Output() onDecline = new EventEmitter();
  isOpen: boolean = false;

  get isInfo() : boolean {
    return this.modalType.toLocaleLowerCase() === 'info';
  };

  get isConfirmation() : boolean {
    return this.modalType.toLowerCase() === 'confirmation';
  };
  
  constructor() { }

  open() {
    this.isOpen = true;
    const modalElement = this.modalElementRef.nativeElement;
    modalElement?.classList.add('show');
    modalElement?.setAttribute('aria-modal', 'true');
    modalElement?.setAttribute('style', 'display: block');
  }
  
  close(){
    this.isOpen = false;
    const modalElement = this.modalElementRef.nativeElement;
    modalElement?.classList.remove('show');
    modalElement?.setAttribute('aria-modal', 'false');
    modalElement?.setAttribute('style', 'display: none');
  }

  accept() {
    this.close();
    this.onAccept.emit();
  }

  decline() {
    this.close();
    this.onDecline.emit();
  }

}