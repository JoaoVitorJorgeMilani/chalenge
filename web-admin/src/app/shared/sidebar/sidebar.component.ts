import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent {
  
  @Input() options: { name: string; route: string; icon: string }[] = [];

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {}

}


