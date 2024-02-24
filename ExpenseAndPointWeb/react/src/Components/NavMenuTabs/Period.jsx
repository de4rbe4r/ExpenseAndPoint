import { React, useState, useEffect } from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { GetExpensesListByUserIdAndPeriodUrl, GetCategoryByIdUrl } from "../../Urls/UrlList";
import Cookies from 'universal-cookie';
import CategoryForm from '../Category/CategoryForm';
import ExpensesList from '../Expense/ExpensesList.jsx';
import DayList from '../DayList.jsx';
import AlertModal from '../AlertModal.jsx';
import axios from 'axios';
import Button from 'react-bootstrap/Button';
import useWindowDimensions from '../../Hooks/useWindowDimensions.jsx'
import Form from 'react-bootstrap/Form';
import ExpenseForm from '../Expense/ExpenseForm';

const Period = () => {
    const cookies = new Cookies();
    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };
    var today = new Date();
    var day = today.getDate();
    if (day < 10) {
        day = '0' + day;
    }
    var month = today.getMonth() + 1;
    if (month < 10) {
        month = '0' + month;
    }
    var year = today.getFullYear();
    const { height, width } = useWindowDimensions();

    const [expensesDict, setExpensesDict] = useState({})
    const [totalAmountExpenses, setTotalAmountExpenses] = useState(null);
    const [expensesList, setExpensesList] = useState([]);
    const [fullExpensesList, setFullExpensesList] = useState(null);
    const [categoryList, setCategoryList] = useState([]);
    const [isDataUpdated, setDataUpdated] = useState(false);
    const [showForm, setShowForm] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");
    const [pickedDate, setPickedDate] = useState();
    const [dateStart, setDateStart] = useState(year + '-' + month + '-' + day);
    const [dateEnd, setDateEnd] = useState(year + '-' + month + '-' + day);
    const [daysWithAmounts, setDaysWithAmounts] = useState({
        days: [],
        amounts: []
    })
    const [showCategoryForm, setShowCategoryForm] = useState(false);

    const [userId, setUserId] = useState(cookies.get('userId'));

    useEffect(() => {
        axios.get(GetCategoryByIdUrl + userId, config)
            .then(res => {
                setCategoryList(res.data)
            })
            .catch(function (error) {
                if (error.response) {
                    setShowAlertModal(true);
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                    setTitleAlert("Ошибка");
                }
            });
    }, [isDataUpdated]);

    useEffect(() => {
        axios.post(GetExpensesListByUserIdAndPeriodUrl, getRequest, config)
            .then(res => {
                setFullExpensesList(res.data);
                var temp = [{}];
                var totalAmount = 0;
                var daysArray = [];
                var amountsArray = [];
                var iterator = 0;
                res.data.map((exp, index) => {
                    if (temp[exp.dateTime.substr(0, 10)] !== undefined) {
                        temp[exp.dateTime.substr(0, 10)].push(exp);
                        amountsArray[iterator - 1] += exp.amount;
                    } else {
                        temp[exp.dateTime.substr(0, 10)] = [exp];
                        daysArray.push(exp.dateTime.substr(0, 10));
                        amountsArray.push(exp.amount);
                        iterator++;
                    }
                    totalAmount += totalAmount + exp.amount;
                })
                setExpensesDict(temp);
                setTotalAmountExpenses(totalAmount);
                setDaysWithAmounts({ amounts: amountsArray, days: daysArray })
            })
            .catch(function (error) {
                if (error.response) {
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                }
            });
    }, [isDataUpdated, dateStart, dateEnd]);

    useEffect(() => {
        setExpensesList(expensesDict[pickedDate]);
    }, [expensesDict]);

    var getRequest = {
        dateStart: dateStart,
        dateEnd: dateEnd,
        userId: userId,
    }

    const setIsShowForm = (data) => {
        setShowForm(data);
    }

    const setIsDataUpdated = (data) => {
        setDataUpdated(data);
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    const openForm = (event) => {
        event.preventDefault();
        setShowForm(true);
    }

    const showExpensesListByDate = (date) => {
        setPickedDate(date);
        setExpensesList(expensesDict[date]);
    }

    const openCategoryForm = (event) => {
        event.preventDefault();
        setShowCategoryForm(true);
    }

    const setIsShowCategoryForm = (data) => {
        setShowCategoryForm(data);
    }

    //if (fullExpensesList === null) return <img src="/loading.gif"></img>;

    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
            <Container>
                <ExpenseForm action="Добавить" setIsDataUpdated={setIsDataUpdated} categoryList={categoryList} expenseToEdit={undefined}
                    isShowForm={showForm} setIsShowForm={setIsShowForm} userId={userId} isDataUpdated={isDataUpdated} />
                <CategoryForm action="Добавить" categoryToEdit={undefined}
                    isShowForm={showCategoryForm} setIsShowForm={setIsShowCategoryForm} userId={userId} setIsDataUpdated={setIsDataUpdated} isDataUpdated={isDataUpdated} />
                <Row>
                    <Col sm={12}>
                        <Row>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" disabled>
                                    Дата начала периода
                                </Button>
                            </Col>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" disabled>
                                    Дата конца периода
                                </Button>
                            </Col>
                        </Row>
                        <Row>
                        <p></p>
                            <Col className="d-grid">
                                <Form.Control
                                    type="date"
                                    defaultValue={dateStart}
                                    onChange={(event) => {
                                        setDateStart(event.target.value);
                                    }}
                                />
                            </Col>
                            <Col className="d-grid">
                                <Form.Control
                                    type="date"
                                    defaultValue={dateEnd}
                                    onChange={(event) => {
                                        setDateEnd(event.target.value);
                                    }}
                                />
                            </Col>
                        </Row>
                        <Row>
                            <p></p>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" onClick={openForm}>Добавить расход</Button>
                            </Col>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" onClick={openCategoryForm}>Добавить категорию</Button>
                            </Col>
                        </Row>
                        <Row>
                            <p></p>
                            <Col className="d-grid">
                            <Button variant="outline-light" size="lg" disabled>
                                    Итого за период {totalAmountExpenses} &#8381;
                            </Button>
                            </Col>
                        </Row>
                        <Row>
                        <p></p>
                            <Col className="d-grid">
                                <DayList sendClickedDate={showExpensesListByDate} height={0.53 * height} dateStart={dateStart} dateEnd={dateEnd} daysWithAmounts={daysWithAmounts} />
                            </Col>
                            <Col className="d-grid">
                                <ExpensesList expensesList={expensesList} categoryList={categoryList} userId={userId} setIsDataUpdatedToParent={setIsDataUpdated}
                                    isDataUpdated={isDataUpdated} height={0.53 * height} date={pickedDate} />
                            </Col>
                        </Row>
                    </Col>
                </Row>
            </Container>
        </>
    );
}

export default Period;