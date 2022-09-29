import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  visitorsCount: number = +localStorage.getItem("numOfVisitors")!;

  constructor() { }

  ngOnInit(): void {
    if (this.visitorsCount === null) {
      localStorage.setItem("numOfVisitors", "0")
    } else {
      this.visitorsCount++;
      localStorage.setItem("numOfVisitors", this.visitorsCount.toString());
    }
  }

}
