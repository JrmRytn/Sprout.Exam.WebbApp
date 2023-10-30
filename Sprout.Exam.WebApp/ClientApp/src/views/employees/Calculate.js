import React, { Component } from "react";
import authService from "../../components/api-authorization/AuthorizeService";

export class EmployeeCalculate extends Component {
  static displayName = EmployeeCalculate.name;

  constructor(props) {
    super(props);
    this.state = {
      id: 0,
      fullName: "",
      birthdate: "",
      tin: "",
      typeId: 1,
      absentDays: 0,
      workedDays: 0,
      workedHours: 0,
      netIncome: 0,
      loading: true,
      loadingCalculate: false,
      validationErrors: {},
    };
  }

  componentDidMount() {
    this.getEmployee(this.props.match.params.id);
  }
  handleChange(event) {
    // this.setState({ [event.target.name]: event.target.value });
    const { name, value } = event.target;

    // Regular expression to match decimal numbers (including integers)
    const decimalRegex = /^-?\d+(\.\d{0,2})?$/;

    // Validate the input against the regex
    if (decimalRegex.test(value) || value === "") {
      this.setState({ [name]: value });
    }
  }

  handleSubmit(e) {
    e.preventDefault();
    this.calculateSalary();
  }

  render() {
    
    const { validationErrors } = this.state;
    const fieldHasError = (fieldName) =>
      validationErrors[fieldName] && validationErrors[fieldName].length > 0;

    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      <div>
        <form>
          <div className="form-row">
            <div className="form-group col-md-12">
              <label>
                Full Name: <b>{this.state.fullName}</b>
              </label>
            </div>
          </div>

          <div className="form-row">
            <div className="form-group col-md-12">
              <label>
                Birthdate: <b>{this.state.birthdate}</b>
              </label>
            </div>
          </div>

          <div className="form-row">
            <div className="form-group col-md-12">
              <label>
                TIN: <b>{this.state.tin}</b>
              </label>
            </div>
          </div>

          <div className="form-row">
            <div className="form-group col-md-12">
              <label>
                Employee Type:{" "}
                <b>
                  {this.state.typeId === 1
                    ? "Regular"
                    : this.state.typeId === 2
                    ? "Contractual"
                    : this.state.typeId === 3
                    ? "Probationary"
                    : "Part-time"}
                </b>
              </label>
            </div>
          </div>

          {this.state.typeId === 1 || this.state.typeId === 3 ? (
            <div className="form-row">
              <div className="form-group col-md-12">
                <label>Salary: 20000 </label>
              </div>
              <div className="form-group col-md-12">
                <label>Tax: 12% </label>
              </div>
            </div>
          ) : this.state.typeId === 2 ? (
            <div className="form-row">
              <div className="form-group col-md-12">
                <label>Rate Per Day: 500 </label>
              </div>
            </div>
          ) : (
            <div className="form-row">
              <div className="form-group col-md-12">
                <label>Rate Per Hour: 50 </label>
              </div>
            </div>
          )}

          <div className="form-row">
            {this.state.typeId === 1 || this.state.typeId === 3 ? (
              <div className="form-group col-md-6">
                <label htmlFor="inputAbsentDays4">Absent Days: </label>
                <input
                  type="text"
                  className="form-control"
                  id="inputAbsentDays4"
                  onChange={this.handleChange.bind(this)}
                  value={this.state.absentDays}
                  name="absentDays"
                  placeholder="Absent Days"
                />
                <span className="text-danger field-validatin-error">
                  {fieldHasError("AbsentDays") && (
                    <span className="error-message">
                      {validationErrors["AbsentDays"][0]}
                    </span>
                  )}
                </span>
              </div>
            ) : this.state.typeId === 2 ? (
              <div className="form-group col-md-6">
                <label htmlFor="inputWorkDays4">Worked Days: </label>
                <input
                  type="text"
                  className="form-control"
                  id="inputWorkDays4"
                  onChange={this.handleChange.bind(this)}
                  value={this.state.workedDays}
                  name="workedDays"
                  placeholder="Worked Days"
                />
                <span className="text-danger field-validatin-error">
                  {fieldHasError("WorkedDays") && (
                    <span className="error-message">
                      {validationErrors["WorkedDays"][0]}
                    </span>
                  )}
                </span>
              </div>
            ) : (
              <div className="form-group col-md-6">
                <label htmlFor="inputHours4">Worked Hours: </label>
                <input
                  type="text"
                  className="form-control"
                  id="inputHours4"
                  onChange={this.handleChange.bind(this)}
                  value={this.state.workedHours}
                  name="workedHours"
                  placeholder="Worked Hours"
                />
                <span className="text-danger field-validatin-error">
                  {fieldHasError("WorkedHours") && (
                    <span className="error-message">
                      {validationErrors["WorkedHours"][0]}
                    </span>
                  )}
                </span>
              </div>
            )}
          </div>

          <div className="form-row">
            <div className="form-group col-md-12">
              <label>
                Net Income:{" "}
                <b>
                  {this.state.netIncome.toLocaleString("en-US", {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2,
                  })}
                </b>
              </label>
            </div>
          </div>

          <button
            type="submit"
            onClick={this.handleSubmit.bind(this)}
            disabled={this.state.loadingCalculate}
            className="btn btn-primary mr-2"
          >
            {this.state.loadingCalculate ? "Loading..." : "Calculate"}
          </button>
          <button
            type="button"
            onClick={() => this.props.history.push("/employees/index")}
            className="btn btn-primary"
          >
            Back
          </button>
        </form>
      </div>
    );

    return (
      <div>
        <h1 id="tabelLabel">Employee Calculate Salary</h1>
        <br />
        {contents}
      </div>
    );
  }

  async calculateSalary() {
    this.setState({ loadingCalculate: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
      method: "POST",
      headers: !token
        ? {}
        : {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
      body: JSON.stringify(this.state),
    };
    const response = await fetch(
      "api/employees/" + this.state.id + "/calculate",
      requestOptions
    );
    if (response.status === 200) {
      const data = await response.json();
      this.setState({ loadingCalculate: false, netIncome: data,validationErrors: {} }); 

    } else if (response.status === 400) { 
      const errorResponse = await response.json();
      this.setState({ validationErrors: errorResponse.errors }); 
      this.setState({ loadingCalculate: false });
    } else {
      alert("There was an error occured.");
      this.setState({ loadingCalculate: false });
    }
  }

  async getEmployee(id) {
    this.setState({ loading: true, loadingCalculate: false });
    const token = await authService.getAccessToken();
    const response = await fetch("api/employees/" + id, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });

    if (response.status === 200) {
      const data = await response.json();
      this.setState({
        id: data.id,
        fullName: data.fullName,
        birthdate: data.birthdate,
        tin: data.tin,
        typeId: data.typeId,
        loading: false,
        loadingCalculate: false,
      });
    } else {
      alert("There was an error occured.");
      this.setState({ loading: false, loadingCalculate: false });
    }
  }
}
